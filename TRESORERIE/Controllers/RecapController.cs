using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TRESORERIE.Models;

namespace TRESORERIE.Controllers
{
    public class RecapController : Controller
    {
        //
        // GET: /Recap/
        TresorerieEntities BD = new TresorerieEntities();
        public Boolean Filter()
        {
            return Session["Filter"] != null;
        }
        public ActionResult FacturationProjet(string Parampassed)
        {
            if (Filter())
            {
                int SelectedSociete = int.Parse(Session["Filter"].ToString());
                List<FACTURATIONS> Liste = new List<FACTURATIONS>();
                if (!string.IsNullOrEmpty(Parampassed) && Parampassed != "null")
                {
                    Liste = BD.FACTURATIONS.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
                    int SelectedProject = int.Parse(Parampassed);
                    Liste = Liste.Where(Element => Element.PROJETS.ID == SelectedProject).ToList();
                    ViewBag.CODE = BD.PROJETS.Find(SelectedProject).CODE;
                    ViewBag.NOM_PROJET = BD.PROJETS.Find(SelectedProject).NOM_PROJET;
                    ViewBag.CLIENT = BD.PROJETS.Find(SelectedProject).TIERS.RAISON_SOCIALE;
                    ViewBag.DEBUT = BD.PROJETS.Find(SelectedProject).DEBUT.ToShortDateString();
                    ViewBag.FIN = BD.PROJETS.Find(SelectedProject).FIN.ToShortDateString();
                    ViewBag.MONTANT_HT = BD.PROJETS.Find(SelectedProject).MONTANT_HT;
                    decimal TOTALF = 0;
                    decimal TOTALR = 0;

                    foreach (FACTURATIONS Element in Liste)
                    {
                        if((Boolean)Element.ETAT_FACTURATION)
                            TOTALF += (decimal)Element.MONTANT_HT;
                        if ((Boolean)Element.ETAT_REGLEMENT)
                            TOTALR += (decimal)Element.MONTANT_HT;
                    }
                    ViewBag.TOTALF = TOTALF;
                    ViewBag.TOTALR = TOTALR;
                    int RapportF = 0;
                    int RapportR = 0;

                    decimal MONTANT_HT = (decimal)BD.PROJETS.Find(SelectedProject).MONTANT_HT;
                    if (MONTANT_HT != 0)
                    {
                        RapportF = (int)((TOTALF * 100) / MONTANT_HT);
                        RapportR = (int)((TOTALR * 100) / MONTANT_HT);
                    }
                    ViewBag.RapportF = RapportF;
                    ViewBag.RapportR = RapportR;

                }

                return View(Liste);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult DepenseMois(string Parampassed)
        {
            if (Filter())
            {
                int SelectedSociete = int.Parse(Session["Filter"].ToString());
                List<CENTRES_COUTS> Liste = BD.CENTRES_COUTS.ToList();
                if (!string.IsNullOrEmpty(Parampassed) && Parampassed != "null")
                {
                    var Variable = (from Element in Liste
                                    select new
                                    {
                                        CODE = Element.CODE,
                                        CENTRE = Element.LIBELLE,
                                        JANVIER = CalculDepenseParMois(1, Parampassed, Element.ID).ToString("F3"),
                                        FEVRIER = CalculDepenseParMois(2, Parampassed, Element.ID).ToString("F3"),
                                        MARS = CalculDepenseParMois(3, Parampassed, Element.ID).ToString("F3"),
                                        AVRIL = CalculDepenseParMois(4, Parampassed, Element.ID).ToString("F3"),
                                        MAI = CalculDepenseParMois(5, Parampassed, Element.ID).ToString("F3"),
                                        JUIN = CalculDepenseParMois(6, Parampassed, Element.ID).ToString("F3"),
                                        JUILLET = CalculDepenseParMois(7, Parampassed, Element.ID).ToString("F3"),
                                        AOUT = CalculDepenseParMois(8, Parampassed, Element.ID).ToString("F3"),
                                        SEPTEMBRE = CalculDepenseParMois(9, Parampassed, Element.ID).ToString("F3"),
                                        OCTOBRE = CalculDepenseParMois(10, Parampassed, Element.ID).ToString("F3"),
                                        NOVEMBRE = CalculDepenseParMois(11, Parampassed, Element.ID).ToString("F3"),
                                        DECEMBRE = CalculDepenseParMois(12, Parampassed, Element.ID).ToString("F3")
                                    }).AsEnumerable().Select(c => c.ToExpando());
                    ViewBag.ANNEE = Parampassed;
                    ViewBag.TOT_JANVIER = CalculDepenseParMois(1, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_FEVRIER = CalculDepenseParMois(2, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_MARS = CalculDepenseParMois(3, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_AVRIL = CalculDepenseParMois(4, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_MAI = CalculDepenseParMois(5, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_JUIN = CalculDepenseParMois(6, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_JUILLET = CalculDepenseParMois(7, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_AOUT = CalculDepenseParMois(8, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_SEPTEMBRE = CalculDepenseParMois(9, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_OCTOBRE = CalculDepenseParMois(10, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_NOVEMBRE = CalculDepenseParMois(11, Parampassed, 0).ToString("F3");
                    ViewBag.TOT_DECEMBRE = CalculDepenseParMois(12, Parampassed, 0).ToString("F3");

                    return View(Variable);

                }
                else
                {
                    var Variable = (from Element in Liste
                                    select new
                                    {
                                        CODE = Element.CODE,
                                        CENTRE = Element.LIBELLE,
                                        JANVIER = 0.ToString("F3"),
                                        FEVRIER = 0.ToString("F3"),
                                        MARS = 0.ToString("F3"),
                                        AVRIL = 0.ToString("F3"),
                                        MAI = 0.ToString("F3"),
                                        JUIN = 0.ToString("F3"),
                                        JUILLET = 0.ToString("F3"),
                                        AOUT = 0.ToString("F3"),
                                        SEPTEMBRE = 0.ToString("F3"),
                                        OCTOBRE = 0.ToString("F3"),
                                        NOVEMBRE = 0.ToString("F3"),
                                        DECEMBRE = 0.ToString("F3")
                                    }).AsEnumerable().Select(c => c.ToExpando());
                    return View(Variable);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public decimal CalculDepenseParMois(int mois, string annee, int param)
        {
            decimal Result = 0;
            int year = int.Parse(annee);
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            List<DEPENSES> Liste = BD.DEPENSES.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
            if (param != 0)
            {
                Liste = Liste.Where(Element => Element.CENTRES_COUTS.ID == param).ToList();
            }
            foreach (DEPENSES Element in Liste)
            {
                if (Element.DATE.Month == mois && Element.DATE.Year == year) Result += (decimal)Element.MONTANT_HT;
            }
            return Result;
        }
        public string ChartCategorie(string Parampassed)
        {
            BD.Configuration.ProxyCreationEnabled = false;
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            List<CATEGORIES_CENTRES_COUTS> ListeCategorie = BD.CATEGORIES_CENTRES_COUTS.ToList();
            var variable = (from Element in ListeCategorie
                            select new
                            {
                                CATEGORIE = Element.LIBELLE,
                                JANVIER = CalculDepenseParMoisGroupCategorie(1, Parampassed, Element.ID).ToString("F3"),
                                FEVRIER = CalculDepenseParMoisGroupCategorie(2, Parampassed, Element.ID).ToString("F3"),
                                MARS = CalculDepenseParMoisGroupCategorie(3, Parampassed, Element.ID).ToString("F3"),
                                AVRIL = CalculDepenseParMoisGroupCategorie(4, Parampassed, Element.ID).ToString("F3"),
                                MAI = CalculDepenseParMoisGroupCategorie(5, Parampassed, Element.ID).ToString("F3"),
                                JUIN = CalculDepenseParMoisGroupCategorie(6, Parampassed, Element.ID).ToString("F3"),
                                JUILLET = CalculDepenseParMoisGroupCategorie(7, Parampassed, Element.ID).ToString("F3"),
                                AOUT = CalculDepenseParMoisGroupCategorie(8, Parampassed, Element.ID).ToString("F3"),
                                SEPTEMBRE = CalculDepenseParMoisGroupCategorie(9, Parampassed, Element.ID).ToString("F3"),
                                OCTOBRE = CalculDepenseParMoisGroupCategorie(10, Parampassed, Element.ID).ToString("F3"),
                                NOVEMBRE = CalculDepenseParMoisGroupCategorie(11, Parampassed, Element.ID).ToString("F3"),
                                DECEMBRE = CalculDepenseParMoisGroupCategorie(12, Parampassed, Element.ID).ToString("F3")
                            }).AsEnumerable().Select(c => c.ToExpando());
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string output = jss.Serialize(variable);
            return output;
        }
        public decimal CalculDepenseParMoisGroupCategorie(int mois, string annee, int param)
        {
            decimal Result = 0;
            int year = int.Parse(annee);
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            List<DEPENSES> Liste = BD.DEPENSES.Where(Element => Element.SOCIETES.ID == SelectedSociete).ToList();
            foreach (DEPENSES Element in Liste)
            {
                CENTRES_COUTS SelectedCentreCout = BD.CENTRES_COUTS.Find(Element.CENTRE_COUT);
                if (Element.DATE.Month == mois && Element.DATE.Year == year && SelectedCentreCout.CATEGORIES_CENTRES_COUTS.ID == param) Result += (decimal)Element.MONTANT_HT;
            }
            return Result;
        }
        public ActionResult DepenseBudgetProjet(string Parampassed)
        {
            if (Filter())
            {
                int SelectedSociete = int.Parse(Session["Filter"].ToString());
                List<CENTRES_COUTS> Liste = BD.CENTRES_COUTS.ToList();
                if (!string.IsNullOrEmpty(Parampassed) && Parampassed != "null")
                {
                    var Variable = (from Element in Liste
                                    select new
                                    {
                                        CODE = Element.CODE,
                                        CENTRE = Element.LIBELLE,
                                        DEPENSE = CalculDepense(Parampassed, Element.ID).ToString("F3"),
                                        BUDGET = CalculBudget(Parampassed, Element.ID).ToString("F3"),
                                    }).AsEnumerable().Select(c => c.ToExpando());
                    int SelectedProject = int.Parse(Parampassed);
                    ViewBag.CODE = BD.PROJETS.Find(SelectedProject).CODE;
                    ViewBag.NOM_PROJET = BD.PROJETS.Find(SelectedProject).NOM_PROJET;
                    ViewBag.CLIENT = BD.PROJETS.Find(SelectedProject).CLIENT;
                    ViewBag.DEBUT = BD.PROJETS.Find(SelectedProject).DEBUT.ToShortDateString();
                    ViewBag.FIN = BD.PROJETS.Find(SelectedProject).FIN.ToShortDateString();
                    ViewBag.MONTANT_HT = BD.PROJETS.Find(SelectedProject).MONTANT_HT;
                    ViewBag.TOT_DEPENSE = CalculDepense(Parampassed, 0).ToString("F3");
                    ViewBag.TOT_BUDGET = CalculBudget(Parampassed, 0).ToString("F3");
                    return View(Variable);

                }
                else
                {
                    var Variable = (from Element in Liste
                                    select new
                                    {
                                        CODE = Element.CODE,
                                        CENTRE = Element.LIBELLE,
                                        DEPENSE = 0.ToString("F3"),
                                        BUDGET = 0.ToString("F3"),
                                    }).AsEnumerable().Select(c => c.ToExpando());
                    return View(Variable);
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public decimal CalculDepense(string projet, int centre)
        {
            decimal Result = 0;
            int PROJET = int.Parse(projet);
            if (centre != 0)
            {
                List<DEPENSES> liste = BD.DEPENSES.Where(Element => Element.PROJETS.ID == PROJET && Element.CENTRES_COUTS.ID == centre).ToList();
                foreach (DEPENSES Element in liste)
                {
                    Result += (decimal)Element.MONTANT_HT;
                }
            }
            else
            {
                List<DEPENSES> liste = BD.DEPENSES.Where(Element => Element.PROJETS.ID == PROJET).ToList();
                foreach (DEPENSES Element in liste)
                {
                    Result += (decimal)Element.MONTANT_HT;
                }
            }
            return Result;
        }
        public decimal CalculBudget(string projet, int centre)
        {
            decimal Result = 0;
            int PROJET = int.Parse(projet);
            if (centre != 0)
            {
                List<BUDGETS> liste = BD.BUDGETS.Where(Element => Element.PROJETS.ID == PROJET && Element.CENTRES_COUTS.ID == centre).ToList();
                foreach (BUDGETS Element in liste)
                {
                    Result += (decimal)Element.MONTANT_HT;
                }
            }
            else
            {
                List<BUDGETS> liste = BD.BUDGETS.Where(Element => Element.PROJETS.ID == PROJET).ToList();
                foreach (BUDGETS Element in liste)
                {
                    Result += (decimal)Element.MONTANT_HT;
                }
            }
            return Result;
        }
        public ActionResult PlanFinancementPrevisionel(string Parampassed, string Filterstring)
        {
            if (Filter())
            {
                int SelectedSociete = int.Parse(Session["Filter"].ToString());
                List<CENTRES_COUTS> Liste = BD.CENTRES_COUTS.ToList();
                if ((!string.IsNullOrEmpty(Parampassed) && Parampassed != "null") && (!string.IsNullOrEmpty(Filterstring) && Filterstring != "null"))
                {
                    dynamic Variable = (from Element in Liste
                                        select new
                                        {
                                            CODE = Element.CODE,
                                            CENTRE = Element.LIBELLE,
                                            PRECEDENTA = CalculPrevisionAchatPrecedent(Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            PRECEDENTF = string.Empty,
                                            JANVIERA = CalculPrevisionAchatParMois(1, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            JANVIERF = string.Empty,
                                            FEVRIERA = CalculPrevisionAchatParMois(2, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            FEVRIERF = string.Empty,
                                            MARSA = CalculPrevisionAchatParMois(3, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            MARSF = string.Empty,
                                            AVRILA = CalculPrevisionAchatParMois(4, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            AVRILF = string.Empty,
                                            MAIA = CalculPrevisionAchatParMois(5, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            MAIF = string.Empty,
                                            JUINA = CalculPrevisionAchatParMois(6, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            JUINF = string.Empty,
                                            JUILLETA = CalculPrevisionAchatParMois(7, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            JUILLETF = string.Empty,
                                            AOUTA = CalculPrevisionAchatParMois(8, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            AOUTF = string.Empty,
                                            SEPTEMBREA = CalculPrevisionAchatParMois(9, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            SEPTEMBREF = string.Empty,
                                            OCTOBREA = CalculPrevisionAchatParMois(10, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            OCTOBREF = string.Empty,
                                            NOVEMBREA = CalculPrevisionAchatParMois(11, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            NOVEMBREF = string.Empty,
                                            DECEMBREA = CalculPrevisionAchatParMois(12, Parampassed, Filterstring, Element.ID).ToString("F2"),
                                            DECEMBREF = string.Empty,

                                        }).AsEnumerable().Select(c => c.ToExpando());
                    ViewBag.ANNEE = Parampassed;
                    int SelectedProjet = int.Parse(Filterstring);
                    ViewBag.CODE = BD.PROJETS.Find(SelectedProjet).CODE;
                    ViewBag.NOM_PROJET = BD.PROJETS.Find(SelectedProjet).NOM_PROJET;
                    ViewBag.CLIENT = BD.PROJETS.Find(SelectedProjet).CLIENT;
                    ViewBag.DEBUT = BD.PROJETS.Find(SelectedProjet).DEBUT.ToShortDateString();
                    ViewBag.FIN = BD.PROJETS.Find(SelectedProjet).FIN.ToShortDateString();
                    ViewBag.MONTANT_HT = BD.PROJETS.Find(SelectedProjet).MONTANT_HT;

                    ViewBag.TOT_ACHAT_PRECEDENT = CalculPrevisionAchatPrecedent(Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_PRECEDENT = CalculPrevisionFactPrecedent(Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_JANVIER = CalculPrevisionAchatParMois(1, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_JANVIER = CalculPrevisionFactParMois(1, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_FEVRIER = CalculPrevisionAchatParMois(2, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_FEVRIER = CalculPrevisionFactParMois(2, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_MARS = CalculPrevisionAchatParMois(3, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_MARS = CalculPrevisionFactParMois(3, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_AVRIL = CalculPrevisionAchatParMois(4, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_AVRIL = CalculPrevisionFactParMois(4, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_MAI = CalculPrevisionAchatParMois(5, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_MAI = CalculPrevisionFactParMois(5, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_JUIN = CalculPrevisionAchatParMois(6, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_JUIN = CalculPrevisionFactParMois(6, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_JUILLET = CalculPrevisionAchatParMois(7, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_JUILLET = CalculPrevisionFactParMois(7, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_AOUT = CalculPrevisionAchatParMois(8, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_AOUT = CalculPrevisionFactParMois(8, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_SEPTEMBRE = CalculPrevisionAchatParMois(9, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_SEPTEMBRE = CalculPrevisionFactParMois(9, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_OCTOBRE = CalculPrevisionAchatParMois(10, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_OCTOBRE = CalculPrevisionFactParMois(10, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_NOVEMBRE = CalculPrevisionAchatParMois(11, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_NOVEMBRE = CalculPrevisionFactParMois(11, Parampassed, Filterstring).ToString("F2");

                    ViewBag.TOT_ACHAT_DECEMBRE = CalculPrevisionAchatParMois(12, Parampassed, Filterstring, 0).ToString("F2");
                    ViewBag.TOT_FACT_DECEMBRE = CalculPrevisionFactParMois(12, Parampassed, Filterstring).ToString("F2");

                    return View(Variable);
                }
                else
                {
                    dynamic Variable = (from Element in Liste
                                        select new
                                        {
                                            CODE = Element.CODE,
                                            CENTRE = Element.LIBELLE,
                                            PRECEDENTA = 0.ToString("F2"),
                                            PRECEDENTF = string.Empty,
                                            JANVIERA = 0.ToString("F2"),
                                            JANVIERF = string.Empty,
                                            FEVRIERA = 0.ToString("F2"),
                                            FEVRIERF = string.Empty,
                                            MARSA = 0.ToString("F2"),
                                            MARSF = string.Empty,
                                            AVRILA = 0.ToString("F2"),
                                            AVRILF = string.Empty,
                                            MAIA = 0.ToString("F2"),
                                            MAIF = string.Empty,
                                            JUINA = 0.ToString("F2"),
                                            JUINF = string.Empty,
                                            JUILLETA = 0.ToString("F2"),
                                            JUILLETF = string.Empty,
                                            AOUTA = 0.ToString("F2"),
                                            AOUTF = string.Empty,
                                            SEPTEMBREA = 0.ToString("F2"),
                                            SEPTEMBREF = string.Empty,
                                            OCTOBREA = 0.ToString("F2"),
                                            OCTOBREF = string.Empty,
                                            NOVEMBREA = 0.ToString("F2"),
                                            NOVEMBREF = string.Empty,
                                            DECEMBREA = 0.ToString("F2"),
                                            DECEMBREF = string.Empty,
                                        }).AsEnumerable().Select(c => c.ToExpando());
                    ViewBag.TOT_ACHAT_PRECEDENT = 0.ToString("F2");
                    ViewBag.TOT_FACT_PRECEDENT = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_JANVIER = 0.ToString("F2");
                    ViewBag.TOT_FACT_JANVIER = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_FEVRIER = 0.ToString("F2");
                    ViewBag.TOT_FACT_FEVRIER = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_MARS = 0.ToString("F2");
                    ViewBag.TOT_FACT_MARS = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_AVRIL = 0.ToString("F2");
                    ViewBag.TOT_FACT_AVRIL = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_MAI = 0.ToString("F2");
                    ViewBag.TOT_FACT_MAI = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_JUIN = 0.ToString("F2");
                    ViewBag.TOT_FACT_JUIN = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_JUILLET = 0.ToString("F2");
                    ViewBag.TOT_FACT_JUILLET = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_AOUT = 0.ToString("F2");
                    ViewBag.TOT_FACT_AOUT = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_SEPTEMBRE = 0.ToString("F2");
                    ViewBag.TOT_FACT_SEPTEMBRE = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_OCTOBRE = 0.ToString("F2");
                    ViewBag.TOT_FACT_OCTOBRE = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_NOVEMBRE = 0.ToString("F2");
                    ViewBag.TOT_FACT_NOVEMBRE = 0.ToString("F2");

                    ViewBag.TOT_ACHAT_DECEMBRE = 0.ToString("F2");
                    ViewBag.TOT_FACT_DECEMBRE = 0.ToString("F2");
                    return View(Variable);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public decimal CalculPrevisionAchatParMois(int mois, string annee, string projet, int param)
        {
            decimal Result = 0;
            int year = int.Parse(annee);
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            int SelectedProjet = int.Parse(projet);
            List<ACHATS_PREVISIONNELS> Liste = BD.ACHATS_PREVISIONNELS.Where(Element => Element.SOCIETES.ID == SelectedSociete && Element.PROJETS.ID == SelectedProjet).ToList();
            if (param != 0)
            {
                Liste = Liste.Where(Element => Element.CENTRES_COUTS.ID == param).ToList();
            }
            foreach (ACHATS_PREVISIONNELS Element in Liste)
            {
                if (Element.DATE.Month == mois && Element.DATE.Year == year) Result += (decimal)Element.MONTANT_HT;
            }
            return Result;
        }
        public decimal CalculPrevisionAchatPrecedent(string annee, string projet, int param)
        {
            decimal Result = 0;
            int year = int.Parse(annee);
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            int SelectedProjet = int.Parse(projet);
            List<ACHATS_PREVISIONNELS> Liste = BD.ACHATS_PREVISIONNELS.Where(Element => Element.SOCIETES.ID == SelectedSociete && Element.PROJETS.ID == SelectedProjet).ToList();
            if (param != 0)
            {
                Liste = Liste.Where(Element => Element.CENTRES_COUTS.ID == param).ToList();
            }
            foreach (ACHATS_PREVISIONNELS Element in Liste)
            {
                if (Element.DATE.Year == year - 1) Result += (decimal)Element.MONTANT_HT;
            }
            return Result;
        }
        public decimal CalculPrevisionFactParMois(int mois, string annee, string projet)
        {
            decimal Result = 0;
            int year = int.Parse(annee);
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            int SelectedProjet = int.Parse(projet);
            List<FACTURATIONS_PREVISIONNELS> Liste = BD.FACTURATIONS_PREVISIONNELS.Where(Element => Element.SOCIETES.ID == SelectedSociete && Element.PROJETS.ID == SelectedProjet).ToList();
            foreach (FACTURATIONS_PREVISIONNELS Element in Liste)
            {
                if (Element.DATE.Month == mois && Element.DATE.Year == year) Result += (decimal)Element.MONTANT_HT;
            }
            return Result;
        }
        public decimal CalculPrevisionFactPrecedent(string annee, string projet)
        {
            decimal Result = 0;
            int year = int.Parse(annee);
            int SelectedSociete = int.Parse(Session["Filter"].ToString());
            int SelectedProjet = int.Parse(projet);
            List<FACTURATIONS_PREVISIONNELS> Liste = BD.FACTURATIONS_PREVISIONNELS.Where(Element => Element.SOCIETES.ID == SelectedSociete && Element.PROJETS.ID == SelectedProjet).ToList();
            foreach (FACTURATIONS_PREVISIONNELS Element in Liste)
            {
                if (Element.DATE.Year == year - 1) Result += (decimal)Element.MONTANT_HT;
            }
            return Result;
        }
        public ActionResult Print(string Parampassed, string Filterstring)
        {

            if (Filter())
            {
                int SelectedSociete = int.Parse(Session["Filter"].ToString());
                List<CENTRES_COUTS> Liste = BD.CENTRES_COUTS.ToList();
                if ((!string.IsNullOrEmpty(Parampassed) && Parampassed != "null") && (!string.IsNullOrEmpty(Filterstring) && Filterstring != "null"))
                {
                    int SelectedProjet = int.Parse(Filterstring);
                    dynamic dt = from Element in Liste
                                 select new
                                 {
                                     CODE_PROJET = BD.PROJETS.Find(SelectedProjet).CODE,
                                     DESCRIPTION_PROJET = BD.PROJETS.Find(SelectedProjet).NOM_PROJET,
                                     CLIENT = BD.PROJETS.Find(SelectedProjet).TIERS.RAISON_SOCIALE,
                                     DU = BD.PROJETS.Find(SelectedProjet).DEBUT.ToShortDateString(),
                                     AU = BD.PROJETS.Find(SelectedProjet).FIN.ToShortDateString(),
                                     MONTANT_HT = BD.PROJETS.Find(SelectedProjet).MONTANT_HT,
                                     ANNEE = Parampassed,
                                     CODE_CENTRE = Element.CODE,
                                     LIBELLE_CENTRE = Element.LIBELLE,
                                     PRECEDENT_ACHAT = CalculPrevisionAchatPrecedent(Parampassed, Filterstring, Element.ID),
                                     PRECEDENT_FACT = CalculPrevisionFactPrecedent(Parampassed, Filterstring),
                                     JANVIER_ACHAT = CalculPrevisionAchatParMois(1, Parampassed, Filterstring, Element.ID),
                                     JANVIER_FACT = CalculPrevisionFactParMois(1, Parampassed, Filterstring),
                                     FEVRIER_ACHAT = CalculPrevisionAchatParMois(2, Parampassed, Filterstring, Element.ID),
                                     FEVRIER_FACT = CalculPrevisionFactParMois(2, Parampassed, Filterstring),
                                     MARS_ACHAT = CalculPrevisionAchatParMois(3, Parampassed, Filterstring, Element.ID),
                                     MARS_FACT = CalculPrevisionFactParMois(3, Parampassed, Filterstring),
                                     AVRIL_ACHAT = CalculPrevisionAchatParMois(4, Parampassed, Filterstring, Element.ID),
                                     AVRIL_FACT = CalculPrevisionFactParMois(4, Parampassed, Filterstring),
                                     MAI_ACHAT = CalculPrevisionAchatParMois(5, Parampassed, Filterstring, Element.ID),
                                     MAI_FACT = CalculPrevisionFactParMois(5, Parampassed, Filterstring),
                                     JUIN_ACHAT = CalculPrevisionAchatParMois(6, Parampassed, Filterstring, Element.ID),
                                     JUIN_FACT = CalculPrevisionFactParMois(6, Parampassed, Filterstring),
                                     JUILLET_ACHAT = CalculPrevisionAchatParMois(7, Parampassed, Filterstring, Element.ID),
                                     JUILLET_FACT = CalculPrevisionFactParMois(7, Parampassed, Filterstring),
                                     AOUT_ACHAT = CalculPrevisionAchatParMois(8, Parampassed, Filterstring, Element.ID),
                                     AOUT_FACT = CalculPrevisionFactParMois(8, Parampassed, Filterstring),
                                     SEPTEMBRE_ACHAT = CalculPrevisionAchatParMois(9, Parampassed, Filterstring, Element.ID),
                                     SEPTEMBRE_FACT = CalculPrevisionFactParMois(9, Parampassed, Filterstring),
                                     OCTOBRE_ACHAT = CalculPrevisionAchatParMois(10, Parampassed, Filterstring, Element.ID),
                                     OCTOBRE_FACT = CalculPrevisionFactParMois(10, Parampassed, Filterstring),
                                     NOVEMBRE_ACHAT = CalculPrevisionAchatParMois(11, Parampassed, Filterstring, Element.ID),
                                     NOVEMBRE_FACT = CalculPrevisionFactParMois(11, Parampassed, Filterstring),
                                     DECEMBRE_ACHAT = CalculPrevisionAchatParMois(12, Parampassed, Filterstring, Element.ID),
                                     DECEMBRE_FACT = CalculPrevisionFactParMois(12, Parampassed, Filterstring),

                                 };
                    ReportDocument rptH = new ReportDocument();
                    string FileName = Server.MapPath("/Reports/PlanFinancement.rpt");
                    rptH.Load(FileName);
                    rptH.SetDataSource(dt);
                    Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    return File(stream, "application/pdf");
                }
                else
                {
                    dynamic dt = from Element in Liste
                                 select new
                                 {
                                     CODE_PROJET = "",
                                     DESCRIPTION_PROJET = "",
                                     CLIENT = "",
                                     DU = "",
                                     AU = "",
                                     MONTANT_HT = "",
                                     ANNEE = "",
                                     CODE_CENTRE = Element.CODE,
                                     LIBELLE_CENTRE = Element.LIBELLE,
                                     PRECEDENT_ACHAT = 0,
                                     PRECEDENT_FACT = 0,
                                     JANVIER_ACHAT = 0,
                                     JANVIER_FACT = 0,
                                     FEVRIER_ACHAT = 0,
                                     FEVRIER_FACT = 0,
                                     MARS_ACHAT = 0,
                                     MARS_FACT = 0,
                                     AVRIL_ACHAT = 0,
                                     AVRIL_FACT = 0,
                                     MAI_ACHAT = 0,
                                     MAI_FACT = 0,
                                     JUIN_ACHAT = 0,
                                     JUIN_FACT = 0,
                                     JUILLET_ACHAT = 0,
                                     JUILLET_FACT = 0,
                                     AOUT_ACHAT = 0,
                                     AOUT_FACT = 0,
                                     SEPTEMBRE_ACHAT = 0,
                                     SEPTEMBRE_FACT = 0,
                                     OCTOBRE_ACHAT = 0,
                                     OCTOBRE_FACT = 0,
                                     NOVEMBRE_ACHAT = 0,
                                     NOVEMBRE_FACT = 0,
                                     DECEMBRE_ACHAT = 0,
                                     DECEMBRE_FACT = 0,

                                 };
                    ReportDocument rptH = new ReportDocument();
                    string FileName = Server.MapPath("/Reports/PlanFinancement.rpt");
                    rptH.Load(FileName);
                    rptH.SetDataSource(dt);
                    Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    return File(stream, "application/pdf");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }

}
