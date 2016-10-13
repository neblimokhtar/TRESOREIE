using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TRESORERIE.Models;

namespace TRESORERIE.Controllers
{
    public class HomeController : Controller
    {
        TresorerieEntities BD = new TresorerieEntities();
        public Boolean _Filter()
        {
            return Session["Filter"] != null;
        }
        public ActionResult Index(string Statut)
        {
            ViewBag.Error = !string.IsNullOrEmpty(Statut) ? Statut : string.Empty;
            return View();
        }
        public ActionResult Dashboard(string Filter)
        {
            if (_Filter())
            {
                int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                ViewBag.SOCIETE = SelectedSociete.NOM;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Send()
        {
            string Filter = Request.Params["Filter"] != null ? Request.Params["Filter"].ToString() : string.Empty;
            string Accee = Request.Params["Accee"] != null ? Request.Params["Accee"].ToString() : string.Empty;
            int ID = int.Parse(Filter);
            SOCIETES SelectedSociete = BD.SOCIETES.Find(ID);
            if (SelectedSociete.CODE_ACCES.ToUpper() == Accee.ToUpper())
            {
                Session["Filter"] = Filter;
                Session["SOCIETE"] = SelectedSociete.NOM;
                return RedirectToAction("Dashboard", "Home", new { Filter = Filter });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { @Statut = "Erreur de connexion !" });
            }
        }
        public JsonResult MoiCourantReglement()
        {
            BD.Configuration.ProxyCreationEnabled = false;
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            List<FACTURATIONS> Liste = BD.FACTURATIONS.Where(Element => Element.SOCIETES.ID == SelectedSociete && Element.DATE_ECHEANCE_REGLEMENT.Month == DateTime.Today.Month && Element.DATE_ECHEANCE_REGLEMENT.Year == DateTime.Today.Year).ToList();
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MoiCourantPaiement()
        {
            BD.Configuration.ProxyCreationEnabled = false;
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            List<DEPENSES> Liste = BD.DEPENSES.Where(Element => Element.SOCIETES.ID == SelectedSociete && Element.DATE.Month == DateTime.Today.Month && Element.DATE.Year == DateTime.Today.Year && Element.DATE_PAIEMENT == null).ToList();
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }
        public string GetProjectName(string Code)
        {
            string Result = string.Empty;
            int ID = int.Parse(Code);
            Result = BD.PROJETS.Find(ID).NOM_PROJET;
            return Result;
        }
        public JsonResult GetAllCategorie()
        {
            BD.Configuration.ProxyCreationEnabled = false;
            List<CATEGORIES_CENTRES_COUTS> liste = BD.CATEGORIES_CENTRES_COUTS.ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
        public string _JsonString(string categorie)
        {
            string Result = string.Empty;
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            int SelectedCategorie=int.Parse(categorie);
            List<ChartProjet> Liste=new List<ChartProjet>();
            List<PROJETS> listeprojet = BD.PROJETS.Where(Element => Element.SOCIETES.ID == SelectedSociete).OrderBy(Element=>Element.DEBUT).ToList();
            foreach (PROJETS Element in listeprojet)
            {
                ChartProjet NewElement = new ChartProjet();
                NewElement.categorie = BD.CATEGORIES_CENTRES_COUTS.Find(SelectedCategorie).LIBELLE;
                NewElement.Projet = Element.NOM_PROJET;
                NewElement.depense = 0;
                List<DEPENSES> listedepense = BD.DEPENSES.Where(Dep => Dep.PROJETS.ID == Element.ID).ToList();
                foreach (DEPENSES depense in listedepense)
                {
                    if (depense.CENTRES_COUTS.CATEGORIES_CENTRES_COUTS.ID == SelectedCategorie)
                        NewElement.depense += depense.MONTANT_HT;
                }
                NewElement.budget = 0;
                List<BUDGETS> listebudget = BD.BUDGETS.Where(Dep => Dep.PROJETS.ID == Element.ID).ToList();
                foreach (BUDGETS budget in listebudget)
                {
                    if(budget.CENTRES_COUTS.CATEGORIES_CENTRES_COUTS.ID==SelectedCategorie)
                    NewElement.budget += budget.MONTANT_HT;
                }
                Liste.Add(NewElement);
            }
            var Variable = from Element in Liste
                           select new
                           {
                               categorie = Element.categorie,
                               Projet = Element.Projet,
                               depense = Element.depense,
                               budget = Element.budget,

                           };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Result = jss.Serialize(Variable);
            return Result;
        }
    }
    public class ChartProjet
    {
        public string categorie;
        public string Projet;
        public decimal depense;
        public decimal budget;
    }
}
