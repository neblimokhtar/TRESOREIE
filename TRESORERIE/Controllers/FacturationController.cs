using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRESORERIE.Models;


namespace TRESORERIE.Controllers
{
    public class FacturationController : Controller
    {
        //
        // GET: /Facturation/
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
                List<FACTURATIONS> Liste = BD.FACTURATIONS.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
                if (!string.IsNullOrEmpty(Parampassed) && Parampassed != "null")
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
        public ActionResult Form(string Mode, string Code)
        {
            if (Filter())
            {
                FACTURATIONS Element = new FACTURATIONS();
                if (Mode == "Create")
                {
                    ViewBag.Titre = "Ajouter une facturation projet";
                    ViewBag.DATE_FACTURATION = DateTime.Today.ToShortDateString();
                    ViewBag.DATE_ECHEANCE_REGLEMENT = DateTime.Today.ToShortDateString();
                    ViewBag.DATE_REGLEMENT_REEL = string.Empty;

                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    Element = BD.FACTURATIONS.Find(CodeInt);
                    ViewBag.Titre = "Modifier une facturation projet";
                    ViewBag.DATE_FACTURATION = Element.DATE_FACTURATION.Value.ToShortDateString();
                    ViewBag.DATE_ECHEANCE_REGLEMENT = Element.DATE_ECHEANCE_REGLEMENT.ToShortDateString();
                    ViewBag.DATE_REGLEMENT_REEL = Element.DATE_REGLEMENT_REEL!=null ? ((DateTime)Element.DATE_REGLEMENT_REEL).ToShortDateString():string.Empty;
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
                string DATE_FACTURATION = Request.Params["DATE_FACTURATION"] != null ? Request.Params["DATE_FACTURATION"].ToString() : string.Empty;
                string DATE_ECHEANCE_REGLEMENT = Request.Params["DATE_ECHEANCE_REGLEMENT"] != null ? Request.Params["DATE_ECHEANCE_REGLEMENT"].ToString() : "";
                string TYPE_PIECE = Request.Params["TYPE_PIECE"] != null ? Request.Params["TYPE_PIECE"].ToString() : string.Empty;
                string NUMERO_PIECE = Request.Params["NUMERO_PIECE"] != null ? Request.Params["NUMERO_PIECE"].ToString() : string.Empty;
                string LIBELLE = Request.Params["LIBELLE"] != null ? Request.Params["LIBELLE"].ToString() : "";
                string MONTANT_HT = Request.Params["MONTANT_HT"] != null ? Request.Params["MONTANT_HT"].ToString() : "0";
                string TVA = Request.Params["TVA"] != null ? Request.Params["TVA"].ToString() : "0";
                string TIMBRE = Request.Params["TIMBRE"] != null ? Request.Params["TIMBRE"].ToString() : "0";
                string FODEC = Request.Params["FODEC"] != null ? Request.Params["FODEC"].ToString() : "0";
                string TTC = Request.Params["TTC"] != null ? Request.Params["TTC"].ToString() : "0";
                string RETENUE_SOURCE = Request.Params["RETENUE_SOURCE"] != null ? Request.Params["RETENUE_SOURCE"].ToString() : "0";
                string GARANTIE = Request.Params["GARANTIE"] != null ? Request.Params["GARANTIE"].ToString() : "0";
                string DATE_REGLEMENT_REEL = Request.Params["DATE_REGLEMENT_REEL"] != null ? Request.Params["DATE_REGLEMENT_REEL"].ToString() : string.Empty;
                string ETAT_FACTURATION = Request.Params["ETAT_FACTURATION"] != null ? Request.Params["ETAT_FACTURATION"].ToString() : string.Empty;
                string ETAT_REGLEMENT = Request.Params["ETAT_REGLEMENT"] != null ? Request.Params["ETAT_REGLEMENT"].ToString() : "";
                if (Mode == "Create")
                {
                    FACTURATIONS NewElement = new FACTURATIONS();
                    int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                    SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                    NewElement.SOCIETES = SelectedSociete;
                    NewElement.SOCIETE = SelectedSocieteID;
                    int SelectedProjetID = int.Parse(Projet_ID);
                    PROJETS SelectedProjet = BD.PROJETS.Find(SelectedProjetID);
                    NewElement.PROJET = SelectedProjetID;
                    NewElement.PROJETS = SelectedProjet;
                    NewElement.DATE_FACTURATION = DateTime.Parse(DATE_FACTURATION);
                    NewElement.DATE_ECHEANCE_REGLEMENT = DateTime.Parse(DATE_ECHEANCE_REGLEMENT);
                    NewElement.TYPE_PIECE = TYPE_PIECE;
                    NewElement.NUMERO_PIECE = NUMERO_PIECE;
                    NewElement.LIBELLE = LIBELLE;
                    NewElement.MONTANT_HT = decimal.Parse(MONTANT_HT, CultureInfo.InvariantCulture);
                    NewElement.TVA = decimal.Parse(TVA, CultureInfo.InvariantCulture);
                    NewElement.TIMBRE = decimal.Parse(TIMBRE, CultureInfo.InvariantCulture);
                    NewElement.FODEC = decimal.Parse(FODEC, CultureInfo.InvariantCulture);
                    NewElement.RETENUE_SOURCE = decimal.Parse(RETENUE_SOURCE, CultureInfo.InvariantCulture);
                    NewElement.TTC = decimal.Parse(TTC, CultureInfo.InvariantCulture);
                    NewElement.GARANTIE = decimal.Parse(GARANTIE, CultureInfo.InvariantCulture);
                    DateTime dateValue;
                    if (DateTime.TryParse(DATE_REGLEMENT_REEL, out dateValue))
                    {
                        NewElement.DATE_REGLEMENT_REEL = DateTime.Parse(DATE_REGLEMENT_REEL);
                    }
                    NewElement.ETAT_FACTURATION = (ETAT_FACTURATION.ToUpper() == "TRUE" || ETAT_FACTURATION.ToUpper() == "ON") ? true : false;
                    NewElement.ETAT_REGLEMENT = (ETAT_REGLEMENT.ToUpper() == "TRUE" || ETAT_REGLEMENT.ToUpper() == "ON") ? true : false;
                    BD.FACTURATIONS.Add(NewElement);
                    BD.SaveChanges();
                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    FACTURATIONS SelectedElement = BD.FACTURATIONS.Find(CodeInt);
                    int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                    SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                    SelectedElement.SOCIETES = SelectedSociete;
                    SelectedElement.SOCIETE = SelectedSocieteID;
                    int SelectedProjetID = int.Parse(Projet_ID);
                    PROJETS SelectedProjet = BD.PROJETS.Find(SelectedProjetID);
                    SelectedElement.PROJET = SelectedProjetID;
                    SelectedElement.PROJETS = SelectedProjet;
                    SelectedElement.DATE_FACTURATION = DateTime.Parse(DATE_FACTURATION);
                    SelectedElement.DATE_ECHEANCE_REGLEMENT = DateTime.Parse(DATE_ECHEANCE_REGLEMENT);
                    SelectedElement.TYPE_PIECE = TYPE_PIECE;
                    SelectedElement.NUMERO_PIECE = NUMERO_PIECE;
                    SelectedElement.LIBELLE = LIBELLE;
                    SelectedElement.MONTANT_HT = decimal.Parse(MONTANT_HT, CultureInfo.InvariantCulture);
                    SelectedElement.TVA = decimal.Parse(TVA, CultureInfo.InvariantCulture);
                    SelectedElement.TIMBRE = decimal.Parse(TIMBRE, CultureInfo.InvariantCulture);
                    SelectedElement.FODEC = decimal.Parse(FODEC, CultureInfo.InvariantCulture);
                    SelectedElement.RETENUE_SOURCE = decimal.Parse(RETENUE_SOURCE, CultureInfo.InvariantCulture);
                    SelectedElement.TTC = decimal.Parse(TTC, CultureInfo.InvariantCulture);
                    SelectedElement.GARANTIE = decimal.Parse(GARANTIE, CultureInfo.InvariantCulture);
                    DateTime dateValue;
                    if (DateTime.TryParse(DATE_REGLEMENT_REEL, out dateValue))
                    {
                        SelectedElement.DATE_REGLEMENT_REEL = DateTime.Parse(DATE_REGLEMENT_REEL);
                    } 
                    SelectedElement.ETAT_FACTURATION = (ETAT_FACTURATION.ToUpper() == "TRUE" || ETAT_FACTURATION.ToUpper() == "ON") ? true : false;
                    SelectedElement.ETAT_REGLEMENT = (ETAT_REGLEMENT.ToUpper() == "TRUE" || ETAT_REGLEMENT.ToUpper() == "ON") ? true : false;
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
            FACTURATIONS SelectedElement = BD.FACTURATIONS.Find(CodeInt);
            BD.FACTURATIONS.Remove(SelectedElement);
            BD.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
