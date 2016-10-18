using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRESORERIE.Models;


namespace TRESORERIE.Controllers
{
    public class FacturationPrevisionelleController : Controller
    {
        //
        // GET: /FacturationPrevisionelle/
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
                List<FACTURATIONS_PREVISIONNELS> Liste = BD.FACTURATIONS_PREVISIONNELS.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
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
                FACTURATIONS_PREVISIONNELS Element = new FACTURATIONS_PREVISIONNELS();
                if (Mode == "Create")
                {
                    ViewBag.Titre = "Ajouter une prévision de facturation";
                    ViewBag.DATE = DateTime.Today.ToShortDateString();

                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    Element = BD.FACTURATIONS_PREVISIONNELS.Find(CodeInt);
                    ViewBag.Titre = "Modifier une prévision de facturation";
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
                string DATE = Request.Params["DATE"] != null ? Request.Params["DATE"].ToString() : string.Empty;
                string MONTANT_HT = Request.Params["MONTANT_HT"] != null ? Request.Params["MONTANT_HT"].ToString() : "0";
                if (Mode == "Create")
                {
                    FACTURATIONS_PREVISIONNELS NewElement = new FACTURATIONS_PREVISIONNELS();
                    int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                    SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                    NewElement.SOCIETES = SelectedSociete;
                    NewElement.SOCIETE = SelectedSocieteID;
                    int SelectedProjetID = int.Parse(Projet_ID);
                    PROJETS SelectedProjet = BD.PROJETS.Find(SelectedProjetID);
                    NewElement.PROJET = SelectedProjetID;
                    NewElement.PROJETS = SelectedProjet;
                    NewElement.DATE = DateTime.Parse(DATE);
                    NewElement.MONTANT_HT = decimal.Parse(MONTANT_HT, CultureInfo.InvariantCulture);
                    BD.FACTURATIONS_PREVISIONNELS.Add(NewElement);
                    BD.SaveChanges();
                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    FACTURATIONS_PREVISIONNELS SelectedElement = BD.FACTURATIONS_PREVISIONNELS.Find(CodeInt);
                    int SelectedSocieteID = int.Parse(Session["Filter"].ToString());
                    SOCIETES SelectedSociete = BD.SOCIETES.Find(SelectedSocieteID);
                    SelectedElement.SOCIETES = SelectedSociete;
                    SelectedElement.SOCIETE = SelectedSocieteID;
                    int SelectedProjetID = int.Parse(Projet_ID);
                    PROJETS SelectedProjet = BD.PROJETS.Find(SelectedProjetID);
                    SelectedElement.PROJET = SelectedProjetID;
                    SelectedElement.PROJETS = SelectedProjet;
                    SelectedElement.DATE = DateTime.Parse(DATE);
                    SelectedElement.MONTANT_HT = decimal.Parse(MONTANT_HT, CultureInfo.InvariantCulture);
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
            FACTURATIONS_PREVISIONNELS SelectedElement = BD.FACTURATIONS_PREVISIONNELS.Find(CodeInt);
            BD.FACTURATIONS_PREVISIONNELS.Remove(SelectedElement);
            BD.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
