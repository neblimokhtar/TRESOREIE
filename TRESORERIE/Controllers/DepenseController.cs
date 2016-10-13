using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRESORERIE.Models;


namespace TRESORERIE.Controllers
{
    public class DepenseController : Controller
    {
        //
        // GET: /Depense/
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
                List<DEPENSES> Liste = BD.DEPENSES.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
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
        public ActionResult Form(string Mode, string Code)
        {
            if (Filter())
            {
                DEPENSES Element = new DEPENSES();
                if (Mode == "Create")
                {
                    ViewBag.Titre = "Ajouter une dépense projet";
                    ViewBag.DATE = DateTime.Today.ToShortDateString();
                    ViewBag.DATE_PAIEMENT = string.Empty;

                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    Element = BD.DEPENSES.Find(CodeInt);
                    ViewBag.Titre = "Modifier une dépense projet";
                    ViewBag.DATE = Element.DATE.ToShortDateString();
                    ViewBag.DATE_PAIEMENT = Element.DATE_PAIEMENT != null ? ((DateTime)Element.DATE_PAIEMENT).ToShortDateString() : string.Empty;
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
                string TYPE_DOCUMENT = Request.Params["TYPE_DOCUMENT"] != null ? Request.Params["TYPE_DOCUMENT"].ToString() : string.Empty;
                string NUMERO_DOCUMENT = Request.Params["NUMERO_DOCUMENT"] != null ? Request.Params["NUMERO_DOCUMENT"].ToString() : string.Empty;
                string NUMERO_PIECE_COMPTABLE = Request.Params["NUMERO_PIECE_COMPTABLE"] != null ? Request.Params["NUMERO_PIECE_COMPTABLE"].ToString() : string.Empty;
                string LIBELLE = Request.Params["LIBELLE"] != null ? Request.Params["LIBELLE"].ToString() : string.Empty;
                string MONTANT_HT = Request.Params["MONTANT_HT"] != null ? Request.Params["MONTANT_HT"].ToString() : "0";
                string TVA = Request.Params["TVA"] != null ? Request.Params["TVA"].ToString() : "0";
                string TIMBRE = Request.Params["TIMBRE"] != null ? Request.Params["TIMBRE"].ToString() : "0";
                string FODEC = Request.Params["FODEC"] != null ? Request.Params["FODEC"].ToString() : "0";
                string RETENUE_SOURCE = Request.Params["RETENUE_SOURCE"] != null ? Request.Params["RETENUE_SOURCE"].ToString() : "0";
                string FOURNISSEUR = Request.Params["FOURNISSEUR"] != null ? Request.Params["FOURNISSEUR"].ToString() : string.Empty;
                string DATE_PAIEMENT = Request.Params["DATE_PAIEMENT"] != null ? Request.Params["DATE_PAIEMENT"].ToString() : string.Empty;
                string MODALITE = Request.Params["MODALITE"] != null ? Request.Params["MODALITE"].ToString() : string.Empty;
                if (Mode == "Create")
                {
                    DEPENSES NewElement = new DEPENSES();
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
                    NewElement.TYPE_DOCUMENT = TYPE_DOCUMENT;
                    NewElement.NUMERO_DOCUMENT = NUMERO_DOCUMENT;
                    NewElement.NUMERO_PIECE_COMPTABLE = NUMERO_PIECE_COMPTABLE;
                    NewElement.LIBELLE = LIBELLE;
                    NewElement.MONTANT_HT = decimal.Parse(MONTANT_HT, CultureInfo.InvariantCulture);
                    NewElement.TVA = decimal.Parse(TVA, CultureInfo.InvariantCulture);
                    NewElement.TIMBRE = decimal.Parse(TIMBRE, CultureInfo.InvariantCulture);
                    NewElement.FODEC = decimal.Parse(FODEC, CultureInfo.InvariantCulture);
                    NewElement.RETENUE_SOURCE = decimal.Parse(RETENUE_SOURCE, CultureInfo.InvariantCulture);
                    NewElement.FOURNISSEUR = FOURNISSEUR;
                    DateTime dateValue;
                    if (DateTime.TryParse(DATE_PAIEMENT, out dateValue))
                    {
                        NewElement.DATE_PAIEMENT = DateTime.Parse(DATE_PAIEMENT);
                    }
                    NewElement.MODALITE = MODALITE;
                    BD.DEPENSES.Add(NewElement);
                    BD.SaveChanges();
                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    DEPENSES SelectedElement = BD.DEPENSES.Find(CodeInt);
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
                    SelectedElement.TYPE_DOCUMENT = TYPE_DOCUMENT;
                    SelectedElement.NUMERO_DOCUMENT = NUMERO_DOCUMENT;
                    SelectedElement.NUMERO_PIECE_COMPTABLE = NUMERO_PIECE_COMPTABLE;
                    SelectedElement.LIBELLE = LIBELLE;
                    SelectedElement.MONTANT_HT = decimal.Parse(MONTANT_HT, CultureInfo.InvariantCulture);
                    SelectedElement.TVA = decimal.Parse(TVA, CultureInfo.InvariantCulture);
                    SelectedElement.TIMBRE = decimal.Parse(TIMBRE, CultureInfo.InvariantCulture);
                    SelectedElement.FODEC = decimal.Parse(FODEC, CultureInfo.InvariantCulture);
                    SelectedElement.RETENUE_SOURCE = decimal.Parse(RETENUE_SOURCE, CultureInfo.InvariantCulture);
                    SelectedElement.FOURNISSEUR = FOURNISSEUR;
                    DateTime dateValue;
                    if (DateTime.TryParse(DATE_PAIEMENT, out dateValue))
                    {
                        SelectedElement.DATE_PAIEMENT = DateTime.Parse(DATE_PAIEMENT);
                    }
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
            DEPENSES SelectedElement = BD.DEPENSES.Find(CodeInt);
            BD.DEPENSES.Remove(SelectedElement);
            BD.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
