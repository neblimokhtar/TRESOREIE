using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRESORERIE.Models;

namespace TRESORERIE.Controllers
{
    public class ProjetController : Controller
    {
        //
        // GET: /Projet/
        TresorerieEntities BD = new TresorerieEntities();
        public Boolean Filter()
        {
            return Session["Filter"] != null;
        }
        public ActionResult Index()
        {
            if (Filter())
            {
                int SelectedSociete = int.Parse(Session["Filter"].ToString());
                List<PROJETS> Liste = BD.PROJETS.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
                return View(Liste);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Form(string Mode, string Code)
        {
            if (Filter())
            {
                PROJETS Element = new PROJETS();
                if (Mode == "Create")
                {
                    ViewBag.Titre = "Ajouter un nouveau projet ";
                    ViewBag.Start = DateTime.Today.ToShortDateString();
                    ViewBag.End = DateTime.Today.ToShortDateString();

                }
                if (Mode == "Edit")
                {
                    int CodeInt = int.Parse(Code);
                    Element = BD.PROJETS.Find(CodeInt);
                    ViewBag.Titre = "Modifier un projet ";
                    ViewBag.Start = Element.DEBUT.ToShortDateString();
                    ViewBag.End = Element.FIN.ToShortDateString();
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
        public JsonResult GetAllClient()
        {
            BD.Configuration.ProxyCreationEnabled = false;
            List<TIERS> Liste = BD.TIERS.Where(Element => Element.TYPE == "CLIENT").ToList();
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }
        public void AddClient(string RAISON_SOCIALE)
        {
            TIERS NewElement = new TIERS();
            NewElement.RAISON_SOCIALE = RAISON_SOCIALE;
            NewElement.TYPE = "CLIENT";
            BD.TIERS.Add(NewElement);
            BD.SaveChanges();
        }
        [HttpPost]
        public ActionResult SendForm(string Mode, string Code)
        {
             if (Filter())
            {
            string CODE_PROJET = Request.Params["CODE_PROJET"] != null ? Request.Params["CODE_PROJET"].ToString() : string.Empty;
            string NOM_PROJET = Request.Params["NOM_PROJET"] != null ? Request.Params["NOM_PROJET"].ToString() : string.Empty;
            string TYPE = Request.Params["TYPE"] != null ? Request.Params["TYPE"].ToString() : string.Empty;
            string CLIENT = Request.Params["CLIENT"] != null ? Request.Params["CLIENT"].ToString() : string.Empty;
            string OWNER = Request.Params["OWNER"] != null ? Request.Params["OWNER"].ToString() : string.Empty;
            string DEBUT = Request.Params["DEBUT"] != null ? Request.Params["DEBUT"].ToString() : string.Empty;
            string FIN = Request.Params["FIN"] != null ? Request.Params["FIN"].ToString() : string.Empty;
            string MONTANT_HT = Request.Params["MONTANT_HT"] != null ? Request.Params["MONTANT_HT"].ToString() : "0";
            string TVA = Request.Params["TVA"] != null ? Request.Params["TVA"].ToString() : "0";
            string GARANTIE = Request.Params["GARANTIE"] != null ? Request.Params["GARANTIE"].ToString() : "0";
            string TYPE_FACTURATION = Request.Params["TYPE_FACTURATION"] != null ? Request.Params["TYPE_FACTURATION"].ToString() : string.Empty;
            string MODALITE_FACTURATION = Request.Params["MODALITE_FACTURATION"] != null ? Request.Params["MODALITE_FACTURATION"].ToString() : string.Empty;
            if (Mode == "Create")
            {
                PROJETS NewElement = new PROJETS();
                NewElement.CODE = CODE_PROJET;
                NewElement.NOM_PROJET = NOM_PROJET;
                NewElement.TYPE = TYPE;
                NewElement.CLIENT = int.Parse(CLIENT);
                NewElement.OWNER = int.Parse(OWNER);
                int ClientID = int.Parse(CLIENT);
                int OWNERID = int.Parse(OWNER);
                TIERS tiers = BD.TIERS.Find(ClientID);
                TIERS tiers1 = BD.TIERS.Find(OWNERID);
                NewElement.TIERS = tiers;
                NewElement.TIERS1 = tiers1;
                NewElement.DEBUT = DateTime.Parse(DEBUT);
                NewElement.FIN = DateTime.Parse(FIN);
                NewElement.MONTANT_HT = decimal.Parse(MONTANT_HT);
                NewElement.TVA = decimal.Parse(TVA);
                NewElement.GARANTIE = decimal.Parse(GARANTIE);
                NewElement.TYPE_FACTURATION = TYPE_FACTURATION;
                NewElement.MODALITE_FACTURATION = MODALITE_FACTURATION;
                int SelectedSocieteID=int.Parse(Session["Filter"].ToString());
                SOCIETES SelectedSociete=BD.SOCIETES.Find(SelectedSocieteID);
                NewElement.SOCIETES=SelectedSociete;
                NewElement.SOCIETE=SelectedSocieteID;
                BD.PROJETS.Add(NewElement);
                BD.SaveChanges();
            }
            if (Mode == "Edit")
            {
                int CodeInt = int.Parse(Code);
                PROJETS SelectedElement = BD.PROJETS.Find(CodeInt);
                SelectedElement.CODE = CODE_PROJET;
                SelectedElement.NOM_PROJET = NOM_PROJET;
                SelectedElement.TYPE = TYPE;
                SelectedElement.CLIENT = int.Parse(CLIENT);
                SelectedElement.OWNER = int.Parse(OWNER);
                int ClientID = int.Parse(CLIENT);
                int OWNERID = int.Parse(OWNER);
                TIERS tiers = BD.TIERS.Find(ClientID);
                TIERS tiers1 = BD.TIERS.Find(OWNERID);
                SelectedElement.TIERS = tiers;
                SelectedElement.TIERS1 = tiers1;
                SelectedElement.DEBUT = DateTime.Parse(DEBUT);
                SelectedElement.FIN = DateTime.Parse(FIN);
                SelectedElement.MONTANT_HT = decimal.Parse(MONTANT_HT);
                SelectedElement.TVA = decimal.Parse(TVA);
                SelectedElement.GARANTIE = decimal.Parse(GARANTIE);
                SelectedElement.TYPE_FACTURATION = TYPE_FACTURATION;
                SelectedElement.MODALITE_FACTURATION = MODALITE_FACTURATION;
                int SelectedSocieteID=int.Parse(Session["Filter"].ToString());
                SOCIETES SelectedSociete=BD.SOCIETES.Find(SelectedSocieteID);
                SelectedElement.SOCIETES=SelectedSociete;
                SelectedElement.SOCIETE=SelectedSocieteID;
                BD.SaveChanges();
            }
            return RedirectToAction("Index");
                 } else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Delete(string Code)
        {
            int CodeInt = int.Parse(Code);
            PROJETS SelectedElement = BD.PROJETS.Find(CodeInt);
            BD.PROJETS.Remove(SelectedElement);
            BD.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
