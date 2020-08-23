using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
  [Authorize(Roles = "Administrator")]
  public class AdministratorController : CommonController
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Department(string subject)
    {
      ViewData["subject"] = subject;
      return View();
    }

    public IActionResult Course(string subject, string num)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      return View();
    }

    /*******Begin code to modify********/

    /// <summary>
    /// Returns a JSON array of all the courses in the given department.
    /// Each object in the array should have the following fields:
    /// "number" - The course number (as in 5530)
    /// "name" - The course name (as in "Database Systems")
    /// </summary>
    /// <param name="subject">The department subject abbreviation (as in "CS")</param>
    /// <returns>The JSON result</returns>
    public IActionResult GetCourses(string subject)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from c in db.Courses
                    where c.SubjectAbbreviation == subject
                    select new
                    {
                        number = c.CourseNumber,
                        name = c.Name
                    };

                return Json(query.ToArray());
            }
        }


    


    /// <summary>
    /// Returns a JSON array of all the professors working in a given department.
    /// Each object in the array should have the following fields:
    /// "lname" - The professor's last name
    /// "fname" - The professor's first name
    /// "uid" - The professor's uid
    /// </summary>
    /// <param name="subject">The department subject abbreviation</param>
    /// <returns>The JSON result</returns>
    public IActionResult GetProfessors(string subject)
    {
            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from p in db.Professors
                    where p.SubjectAbbreviation == subject
                    select new
                    {
                        lname = p.LastName,
                        fname = p.FirstName,
                        uid = p.UId
                    };

                return Json(query.ToArray());
            }
    }



    /// <summary>
    /// Creates a course.
    /// A course is uniquely identified by its number + the subject to which it belongs
    /// </summary>
    /// <param name="subject">The subject abbreviation for the department in which the course will be added</param>
    /// <param name="number">The course number</param>
    /// <param name="name">The course name</param>
    /// <returns>A JSON object containing {success = true/false}.
    /// false if the course already exists, true otherwise.</returns>
    public IActionResult CreateCourse(string subject, int number, string name)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                     from c1 in db.Courses
                     where c1.CourseNumber == number && c1.SubjectAbbreviation == subject
                     select c1;

                if (query != null) return Json(new { success = false });
                LMS.Models.LMSModels.Courses c = new Models.LMSModels.Courses();
                c.SubjectAbbreviation = subject;
                c.CourseNumber = (ushort)number;
                c.Name = name;
                db.Courses.Add(c);
                db.SaveChanges();
                return Json(new { success = true });
            }

    }



    /// <summary>
    /// Creates a class offering of a given course.
    /// </summary>
    /// <param name="subject">The department subject abbreviation</param>
    /// <param name="number">The course number</param>
    /// <param name="season">The season part of the semester</param>
    /// <param name="year">The year part of the semester</param>
    /// <param name="start">The start time</param>
    /// <param name="end">The end time</param>
    /// <param name="location">The location</param>
    /// <param name="instructor">The uid of the professor</param>
    /// <returns>A JSON object containing {success = true/false}. 
    /// false if another class occupies the same location during any time 
    /// within the start-end range in the same semester, or if there is already
    /// a Class offering of the same Course in the same Semester,
    /// true otherwise.</returns>
    public IActionResult CreateClass(string subject, int number, string season, int year, DateTime start, DateTime end, string location, string instructor)
    {

            /*
                 var chars = "0123456789";
                 var stringChars = new char[5];
                 var random = new Random();

                 for (int i = 0; i < stringChars.Length; i++)
                 {
                     stringChars[i] = chars[random.Next(chars.Length)];
                 }

                 var classID = new String(stringChars);
                 */
            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query = 
                    from departments in db.Departments
                    join courses in db.Courses
                    on departments.SubjectAbbreviation equals courses.SubjectAbbreviation
                    where courses.SubjectAbbreviation == subject &&
                    courses.CourseNumber == number
                    select courses.CourseNumber;

                Models.LMSModels.Classes c = new Models.LMSModels.Classes();
                c.SemesterYear = (uint)year;
                c.SemesterSeason = season;
                c.StartTime = start;
                c.EndTime = end;
                c.Location = location;
                c.CourseNumber = query.First(); // First or default? Should there always be only 1 here?
                c.ProfessorId = instructor;

                db.Classes.Add(c);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    return Json(new { success = false });
                }

                return Json(new { success = true });
            }

    }


    /*******End code to modify********/

  }
}