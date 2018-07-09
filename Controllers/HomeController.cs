using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DojoDachi.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace DojoDachi.Controllers
{


    public static class SessionExtentions
    {
        public static void SetObjAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }


    public class HomeController : Controller
    {
        Random rand = new Random();

        [HttpGet("")]
        public IActionResult Index(int Fullness = 20, int Happiness = 20, int Meals = 3, int Energy = 50, string Msg = "Welcome")
        {
            if (Fullness <= 0 || Happiness <= 0)
            {

                ViewBag.Fullness = Fullness;
                ViewBag.Happiness = Happiness;
                ViewBag.Meals = Meals;
                ViewBag.Energy = Energy;
                ViewBag.won = false;

                ViewBag.isDead = true;
                ViewBag.Msg = "Dachi Passed Away";
            }

            else if (Fullness >= 100 && Happiness >= 100 && Energy >= 100)
            {
                ViewBag.Fullness = Fullness;
                ViewBag.Happiness = Happiness;
                ViewBag.Meals = Meals;
                ViewBag.Energy = Energy;
                ViewBag.won = true;
                ViewBag.isDead = false;

                ViewBag.Msg = "You Won the Game";

            }

            else
            {

                HttpContext.Session.SetInt32("ful", Fullness);
                HttpContext.Session.SetInt32("hap", Happiness);
                HttpContext.Session.SetInt32("meals", Meals);
                HttpContext.Session.SetInt32("enr", Energy);
                HttpContext.Session.SetString("msg", Msg);

                ViewBag.won = false;
                ViewBag.Fullness = Fullness;
                ViewBag.Happiness = Happiness;
                ViewBag.Meals = Meals;
                ViewBag.Energy = Energy;
                ViewBag.isDead = false;
                ViewBag.Msg = Msg;
            }



            return View();
        }

        [HttpGet("feed")]
        public IActionResult Feed()
        {
            int? happi = HttpContext.Session.GetInt32("hap");
            int? ener = HttpContext.Session.GetInt32("enr");
            int? full = HttpContext.Session.GetInt32("ful");
            int? meals = HttpContext.Session.GetInt32("meals");
            if (meals == 0)
            {

                return RedirectToAction("Index", new { Msg = "Can't Feed No Meals Left", Meals = 0, Fullness = full, Happiness = happi, Energy = ener });
            }
            else
            {
                meals--;
                if (rand.Next(1, 5) > 1)
                {

                    full += rand.Next(5, 11);
                }
                else
                {
                    Console.WriteLine("Didn't like the food");
                }


                return RedirectToAction("Index", new { Fullness = full, Meals = meals, Happiness = happi, Energy = ener, Msg = "Dachi is Eating" });
            }


        }

        [HttpGet("play")]
        public IActionResult Play()
        {
            int? happi = HttpContext.Session.GetInt32("hap");
            int? ener = HttpContext.Session.GetInt32("enr");
            int? full = HttpContext.Session.GetInt32("ful");
            int? meals = HttpContext.Session.GetInt32("meals");
            if (ener == 0)
            {

                return RedirectToAction("Index", new { Msg = "Can't Play no Energy Left", Happiness = happi, Energy = ener, Fullness = full, Meals = meals });
            }
            else
            {
                ener -= 5;
                if (rand.Next(1, 5) > 1)
                {

                    happi += rand.Next(5, 11);

                }
                else
                {
                    Console.WriteLine("Didn't like the play");
                }
                return RedirectToAction("Index", new { Happiness = happi, Energy = ener, Fullness = full, Meals = meals, Msg = "Dachi is Playing " });
            }


        }

        [HttpGet("work")]
        public IActionResult Work()
        {
            int? happi = HttpContext.Session.GetInt32("hap");
            int? ener = HttpContext.Session.GetInt32("enr");
            int? full = HttpContext.Session.GetInt32("ful");
            int? meals = HttpContext.Session.GetInt32("meals");
            if (ener == 0)
            {

                return RedirectToAction("Index", new { Msg = "Can't Work no Energy Left", Happiness = happi, Energy = ener, Fullness = full, Meals = meals });
            }
            else
            {
                ener -= 5;
                meals += rand.Next(1, 4);
                return RedirectToAction("Index", new { Happiness = happi, Energy = ener, Fullness = full, Meals = meals, Msg = "Dachi is Working" });
            }

        }
        [HttpGet("sleep")]
        public IActionResult Sleep()
        {
            int? happi = HttpContext.Session.GetInt32("hap");
            int? ener = HttpContext.Session.GetInt32("enr");
            int? full = HttpContext.Session.GetInt32("ful");
            int? meals = HttpContext.Session.GetInt32("meals");
            if (full <= 0 || happi <= 0)
            {

                return RedirectToAction("Index", new { Msg = "Dachi passed away" });
            }
            else
            {
                ener += 15;
                full -= 5;
                happi -= 5;
                return RedirectToAction("Index", new { Happiness = happi, Energy = ener, Fullness = full, Meals = meals, Msg = "Dachi Sleeping" });
            }

        }

        [HttpGet("restart")]
        public IActionResult Restart()
        {
            return RedirectToAction("Index");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
