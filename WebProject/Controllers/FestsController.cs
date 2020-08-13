using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class FestsController : ControllerBase
    {
        // GET: Fests
        public ActionResult Index()
        {

            Dictionary<string, Manifestation> fests = getManifestations;
            List<Manifestation> list = new List<Manifestation>();

            foreach (var item in fests.Values)
            {
                if (item.DateTime > DateTime.Now)
                {
                    list.Add(item);
                }
            }
            list.Sort((x, y) => DateTime.Compare(x.DateTime, y.DateTime));            
            HttpContext.Cache["tempList"] = list;
            ViewBag.fests = list.Where(x => x.Deleted == false);


            return View();
        }


        public ActionResult Details(string name)
        {
            int rat = 0;
            int count = 0;
            Dictionary<string, double> dic = new Dictionary<string, double>(); 

           
            foreach (var man in getManifestations.Values)
            {
                if (man.Name == name)
                {
                    ViewBag.Fest = man;

                    foreach (var comm in getComments.Values)
                    {
                        if (man.Name == comm.Manifestation)
                        {
                            rat += comm.Rating;
                            count += 1;
                        }
                    }
                    double ratin = (double)rat / count;
                    dic[man.Name] = ratin;
                }                
            }
            ViewBag.rats = dic;
            ViewBag.comments = getComments.Values;
            return View("Manifestation");
        }

        [HttpPost]
        public ActionResult Filter(string radio)
        {
            List<Manifestation> temp = new List<Manifestation>();
            foreach (var it in getList)
            {
                temp.Add(it);
            }

            foreach (var item in getList)
            {
                if (item.Type.ToString() != radio)
                {
                    temp.Remove(item);
                }
            }
            ViewBag.Fests = temp.Where(x => x.Deleted == false);
            return View("Index");
        }


        [HttpPost]
        public ActionResult Sort(string radio)
        {
            var list2 = getList.Where(x => x.Deleted == false);

            switch (radio)
            {
                case "aName":
                    var list = list2.OrderBy(x => x.Name);
                    ViewBag.Fests = list;
                    return View("Index");

                case "dName":
                    ViewBag.Fests = list2.OrderByDescending(x => x.Name); ;
                    return View("Index");

                case "aDate":
                    getList.Sort((x, y) => DateTime.Compare(x.DateTime, y.DateTime));
                    ViewBag.Fests = getList.Where(x => x.Deleted == false);
                    return View("Index");

                case "dDate":
                    getList.Sort((y, x) => DateTime.Compare(x.DateTime, y.DateTime));
                    ViewBag.Fests = getList.Where(x => x.Deleted == false);
                    return View("Index");

                case "dPrice":

                    getList.Sort((y, x) => x.Price.CompareTo(y.Price));
                    ViewBag.Fests = getList.Where(x => x.Deleted == false);
                    return View("Index");

                case "aPrice":
                    getList.Sort((x, y) => x.Price.CompareTo(y.Price));
                    ViewBag.Fests = getList.Where(x => x.Deleted == false);
                    return View("Index");


            }
            ViewBag.Fests = getList.Where(x => x.Deleted == false);
            return View("Index");
        }

        [HttpPost]
        public ActionResult Search(SearchModel sm)
        {
            List<Manifestation> list = new List<Manifestation>();
            foreach (var item in getManifestations.Values)
            {
                list.Add(item);
            }

            if (sm.Name == null && sm.City == null && sm.StartPrice == null && sm.EndPrice == null && sm.StartDate == null && sm.EndDate == null)
            {
                HttpContext.Cache["tempList"] = list;
                ViewBag.Fests = list.Where(x => x.Deleted == false);
                return View("Index");
            }


            foreach (var man in getManifestations.Values)
            {
                if (sm.City != null)
                {
                    if (sm.City != man.Address.City)
                    {
                        list.Remove(man);
                    }
                }
                if (sm.StartPrice > man.Price || sm.EndPrice < man.Price)
                {
                    list.Remove(man);
                }
                if (sm.StartDate > man.DateTime || sm.EndDate < man.DateTime)
                {
                    list.Remove(man);
                }
                if (sm.Name != null)
                {
                    if (sm.Name != man.Name)
                    {
                        list.Remove(man);
                    }
                }
            }
            HttpContext.Cache["tempList"] = list;
            ViewBag.fests = list.Where(x => x.Deleted == false);
            return View("Index");


        }
    }
}