using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRESORERIE.Models;

namespace TRESORERIE.Controllers
{
    public class AchatPrevisionnelController : Controller
    {
        //
        // GET: /AchatPrevisionnel/
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
                List<ACHATS_PREVISIONNELS> Liste = BD.ACHATS_PREVISIONNELS.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
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
        public JsonResult GetAllCentreCout()
        {
            BD.Configuration.ProxyCreationEnabled = false;
            List<CENTRES_COUTS> Liste = BD.CENTRES_COUTS.ToList();
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllFournisseur()
        {
            BD.Configuration.ProxyCreationEnabled = false;
            List<TIERS> Liste = BD.TIERS.Where(Element => Element.TYPE == "FOURNISSEUR").ToList();
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }
        public void AddFournisseur(string RAISON_SOCIALE)
        {
            TIERS NewElement = new TIERS();
            NewElement.RAISON_SOCIALE = RAISON_SOCIALE;
            NewElement.TYPE = "FOURNISSEUR";
            BD.TIERS.Add(NewElement);
            BD.SaveChanges();
        }
        public ActionResult Form(string Mode, string Code)
        {
            if (Filter())
            {
                ACHATS_PREVISIONNELS Element = new ACHATS_PREVISIONNELS();
                if (Mode == "Create")
                {
                    ViewBag.Titre = "Ajouter une prévision d'achat";
                    ViewBag.DATE = DateTime.Today.ToShortDateString();

                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    Element = BD.ACHATS_PREVISIONNELS.Find(CodeInt);
                    ViewBag.Titre = "Modifier une prévision d'achat";
                    ViewBag.DATE = Element.DATE.ToShortDateString();
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
                string CentreCout_ID = Request.Params["CentreCout_ID"] != null ? Request.Params["CentreCout_ID"].ToString() : string.Empty;
                string DATE = Request.Params["DATE"] != null ? Request.Params["DATE"].ToString() : string.Empty;
                string MONTANT_HT = Request.Params["MONTANT_HT"] != null ? Request.Params["MONTANT_HT"].ToString() : "0";
                string FOURNISSEUR = Request.Params["FOURNISSEUR"] != null ? Request.Params["FOURNISSEUR"].ToString() : string.Empty;
                string MODALITE = Request.Params["MODALITE"] != null ? Request.Params["MODALITE"].ToString() : string.Empty;
                if (Mode == "Create")
                {
                    ACHATS_PREVISIONNELS NewElement = new ACHATS_PREVISIONNELS();
                    int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                    SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                    NewElement.SOCIETES = SelectedSociete;
                    NewElement.SOCIETE = SelectedSocieteID;
                    int SelectedProjetID = int.Parse(Projet_ID);
                    PROJETS SelectedProjet = BD.PROJETS.Find(SelectedProjetID);
                    NewElement.PROJET = SelectedProjetID;
                    NewElement.PROJETS = SelectedProjet;
                    int SelectedCentreCoutID = int.Parse(CentreCout_ID);
                    CENTRES_COUTS SelectedCentreCout = BD.CENTRES_COUTS.Find(SelectedCentreCoutID);
                    NewElement.CENTRE_COUT = SelectedCentreCoutID;
                    NewElement.CENTRES_COUTS = SelectedCentreCout;
                    NewElement.DATE = DateTime.Parse(DATE);
                    NewElement.MONTANT_HT = decimal.Parse(MONTANT_HT, CultureInfo.InvariantCulture);
                    NewElement.FOURNISSEUR = int.Parse(FOURNISSEUR);
                    int SelectedFournisseur = int.Parse(FOURNISSEUR);
                    TIERS Tires = BD.TIERS.Find(SelectedFournisseur);
                    NewElement.TIERS = Tires;
                    NewElement.MODALITE = MODALITE;
                    BD.ACHATS_PREVISIONNELS.Add(NewElement);
                    BD.SaveChanges();
                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    ACHATS_PREVISIONNELS SelectedElement = BD.ACHATS_PREVISIONNELS.Find(CodeInt);
                    int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                    SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                    SelectedElement.SOCIETES = SelectedSociete;
                    SelectedElement.SOCIETE = SelectedSocieteID;
                    int SelectedProjetID = int.Parse(Projet_ID);
                    PROJETS SelectedProjet = BD.PROJETS.Find(SelectedProjetID);
                    SelectedElement.PROJET = SelectedProjetID;
                    SelectedElement.PROJETS = SelectedProjet;
                    int SelectedCentreCoutID = int.Parse(CentreCout_ID);
                    CENTRES_COUTS SelectedCentreCout = BD.CENTRES_COUTS.Find(SelectedCentreCoutID);
                    SelectedElement.CENTRE_COUT = SelectedCentreCoutID;
                    SelectedElement.CENTRES_COUTS = SelectedCentreCout;
                    SelectedElement.DATE = DateTime.Parse(DATE);
                    SelectedElement.MONTANT_HT = decimal.Parse(MONTANT_HT, CultureInfo.InvariantCulture);
                    SelectedElement.FOURNISSEUR = int.Parse(FOURNISSEUR);
                    int SelectedFournisseur = int.Parse(FOURNISSEUR);
                    TIERS Tires = BD.TIERS.Find(SelectedFournisseur);
                    SelectedElement.TIERS = Tires;
                    SelectedElement.MODALITE = MODALITE;
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
            ACHATS_PREVISIONNELS SelectedElement = BD.ACHATS_PREVISIONNELS.Find(CodeInt);
            BD.ACHATS_PREVISIONNELS.Remove(SelectedElement);
            BD.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
