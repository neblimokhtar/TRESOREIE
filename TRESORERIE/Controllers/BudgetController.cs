using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRESORERIE.Models;


namespace TRESORERIE.Controllers
{
    public class BudgetController : Controller
    {
        //
        // GET: /Budget/
        TresorerieEntities BD = new TresorerieEntities();
        public Boolean Filter()
        {
            return Session["Filter"] != null;
        }
        public ActionResult Index(string Parampassed)
        {
            if (Filter())
            {
                int SelectedSociete = int.Parse(Session["Filter"].ToString());
                List<BUDGETS> Liste = BD.BUDGETS.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
                if (!string.IsNullOrEmpty(Parampassed) && Parampassed!="null")
                {
                    int SelectedProject = int.Parse(Parampassed);
                    Liste = Liste.Where(Element => Element.PROJETS.ID == SelectedProject).ToList();
                    ViewBag.PROJET = BD.PROJETS.Find(SelectedProject).CODE;
                }

                return View(Liste);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public JsonResult GetAllProject()
        {
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            BD.Configuration.ProxyCreationEnabled = false;
            List<PROJETS> Liste = BD.PROJETS.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllCentre()
        {
            BD.Configuration.ProxyCreationEnabled = false;
            List<CENTRES_COUTS> Liste = BD.CENTRES_COUTS.ToList();
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Form(string Mode, string Code)
        {
            if (Filter())
            {
                BUDGETS Element = new BUDGETS();
                if (Mode == "Create")
                {
                    ViewBag.Titre = "Ajouter un budget prévisionnel";
                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    Element = BD.BUDGETS.Find(CodeInt);
                    ViewBag.Titre = "Modifier un budget prévisionnel";
                }
                ViewBag.Mode = Mode;
                ViewBag.Code = Code;
                return View(Element);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult SendForm(string Mode, string Code)
        {
            if (Filter())
            {
                string Projet_ID = Request.Params["Projet_ID"] != null ? Request.Params["Projet_ID"].ToString() : string.Empty;
                string ContreCout_ID = Request.Params["ContreCout_ID"] != null ? Request.Params["ContreCout_ID"].ToString() : string.Empty;
                string MONTANT_HT = Request.Params["MONTANT_HT"] != null ? Request.Params["MONTANT_HT"].ToString() : "0";
                if (Mode == "Create")
                {
                    BUDGETS NewElement = new BUDGETS();
                    NewElement.MONTANT_HT = decimal.Parse(MONTANT_HT);
                    int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                    SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                    NewElement.SOCIETES = SelectedSociete;
                    NewElement.SOCIETE = SelectedSocieteID;
                    int SelectedCentreCoutID = int.Parse(ContreCout_ID);
                    CENTRES_COUTS SelectedCentreCout = BD.CENTRES_COUTS.Find(SelectedCentreCoutID);
                    NewElement.CENTRE_COUT = SelectedCentreCoutID;
                    NewElement.CENTRES_COUTS = SelectedCentreCout;
                    int SelectedProjetID = int.Parse(Projet_ID);
                    PROJETS SelectedProjet = BD.PROJETS.Find(SelectedProjetID);
                    NewElement.PROJET = SelectedProjetID;
                    NewElement.PROJETS = SelectedProjet;
                    BD.BUDGETS.Add(NewElement);
                    BD.SaveChanges();
                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    BUDGETS SelectedElement = BD.BUDGETS.Find(CodeInt);
                    SelectedElement.MONTANT_HT = decimal.Parse(MONTANT_HT);
                    int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                    SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                    SelectedElement.SOCIETES = SelectedSociete;
                    SelectedElement.SOCIETE = SelectedSocieteID;
                    int SelectedCentreCoutID = int.Parse(ContreCout_ID);
                    CENTRES_COUTS SelectedCentreCout = BD.CENTRES_COUTS.Find(SelectedCentreCoutID);
                    SelectedElement.CENTRE_COUT = SelectedCentreCoutID;
                    SelectedElement.CENTRES_COUTS = SelectedCentreCout;
                    int SelectedProjetID = int.Parse(Projet_ID);
                    PROJETS SelectedProjet = BD.PROJETS.Find(SelectedProjetID);
                    SelectedElement.PROJET = SelectedProjetID;
                    SelectedElement.PROJETS = SelectedProjet;
                    BD.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Delete(string Code)
        {
            int CodeInt = int.Parse(Code);
            BUDGETS SelectedElement = BD.BUDGETS.Find(CodeInt);
            BD.BUDGETS.Remove(SelectedElement);
            BD.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
