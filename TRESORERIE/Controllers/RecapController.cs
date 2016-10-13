using System;
using System.Collections.Generic;
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
                    ViewBag.CLIENT = BD.PROJETS.Find(SelectedProject).CLIENT;
                    ViewBag.DEBUT = BD.PROJETS.Find(SelectedProject).DEBUT.ToShortDateString();
                    ViewBag.FIN = BD.PROJETS.Find(SelectedProject).FIN.ToShortDateString();
                    ViewBag.MONTANT_HT = BD.PROJETS.Find(SelectedProject).MONTANT_HT;
                    decimal TOTAL = 0;
                    foreach (FACTURATIONS Element in Liste)
                    {
                        TOTAL += Element.MONTANT_HT;
                    }
                    ViewBag.TOTAL = TOTAL;
                    int Rapport = 0;
                    decimal MONTANT_HT = (decimal)BD.PROJETS.Find(SelectedProject).MONTANT_HT;
                    if (MONTANT_HT != 0)
                    {
                        Rapport = (int)((TOTAL * 100) / MONTANT_HT);
                    }
                    ViewBag.Rapport = Rapport;

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
                if (Element.DATE.Month == mois && Element.DATE.Year == year) Result += Element.MONTANT_HT;
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
                if (Element.DATE.Month == mois && Element.DATE.Year == year && SelectedCentreCout.CATEGORIES_CENTRES_COUTS.ID == param) Result += Element.MONTANT_HT;
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
                    Result += Element.MONTANT_HT;
                }
            }
            else
            {
                List<DEPENSES> liste = BD.DEPENSES.Where(Element => Element.PROJETS.ID == PROJET).ToList();
                foreach (DEPENSES Element in liste)
                {
                    Result += Element.MONTANT_HT;
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
                    Result += Element.MONTANT_HT;
                }
            }
            else
            {
                List<BUDGETS> liste = BD.BUDGETS.Where(Element => Element.PROJETS.ID == PROJET).ToList();
                foreach (BUDGETS Element in liste)
                {
                    Result += Element.MONTANT_HT;
                }
            }
            return Result;
        }

    }

}
