using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
  public class CommonController : Controller
  {

    /*******Begin code to modify********/

    // TODO: Uncomment and change 'X' after you have scaffoled

    
    protected Models.LMSModels.Team13LMSContext db;

    public CommonController()
    {
      db = new Models.LMSModels.Team13LMSContext();
    }
    

    /*
     * WARNING: This is the quick and easy way to make the controller
     *          use a different LibraryContext - good enough for our purposes.
     *          The "right" way is through Dependency Injection via the constructor 
     *          (look this up if interested).
    */

    // TODO: Uncomment and change 'X' after you have scaffoled
    
    public void UseLMSContext(Models.LMSModels.Team13LMSContext ctx)
    {
      db = ctx;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }




        /// <summary>
        /// Retreive a JSON array of all departments from the database.
        /// Each object in the array should have a field called "name" and "subject",
        /// where "name" is the department name and "subject" is the subject abbreviation.
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetDepartments()
        {
            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from d in db.Departments
                    select d;

                return Json(query.ToArray());
            }
        }



        /// <summary>
        /// Returns a JSON array representing the course catalog.
        /// Each object in the array should have the following fields:
        /// "subject": The subject abbreviation, (e.g. "CS")
        /// "dname": The department name, as in "Computer Science"
        /// "courses": An array of JSON objects representing the courses in the department.
        ///            Each field in this inner-array should have the following fields:
        ///            "number": The course number (e.g. 5530)
        ///            "cname": The course name (e.g. "Database Systems")
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetCatalog()
        {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from d in db.Departments
                    select new
                    {
                        subject = d.SubjectAbbreviation,
                        dname = d.Name,
                        courses =
                            from c in db.Courses
                            where c.SubjectAbbreviation == d.SubjectAbbreviation
                            select new
                            {
                                number = c.CourseNumber,
                                cname = c.Name
                            }

                    };

                return Json(query.ToArray());
            }

        }

        /// <summary>
        /// Returns a JSON array of all class offerings of a specific course.
        /// Each object in the array should have the following fields:
        /// "season": the season part of the semester, such as "Fall"
        /// "year": the year part of the semester
        /// "location": the location of the class
        /// "start": the start time in format "hh:mm:ss"
        /// "end": the end time in format "hh:mm:ss"
        /// "fname": the first name of the professor
        /// "lname": the last name of the professor
        /// </summary>
        /// <param name="subject">The subject abbreviation, as in "CS"</param>
        /// <param name="number">The course number, as in 5530</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetClassOfferings(string subject, int number)
        {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {

                var query =
                    from c1 in db.Courses
                    where c1.CourseNumber == number && c1.SubjectAbbreviation == subject
                    join c2 in db.Classes on c1.CourseNumber equals c2.CourseNumber
                    join prof in db.Professors on c2.ProfessorId equals prof.UId

                    select new
                    {
                        season = c2.SemesterSeason,
                        year = c2.SemesterYear,
                        location = c2.Location,
                        start = c2.StartTime,
                        end = c2.EndTime,
                        fname = prof.FirstName,
                        lname = prof.LastName
                    };


                return Json(query.ToArray());
            }

        }

    /// <summary>
    /// This method does NOT return JSON. It returns plain text (containing html).
    /// Use "return Content(...)" to return plain text.
    /// Returns the contents of an assignment.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment in the category</param>
    /// <returns>The assignment contents</returns>
    public IActionResult GetAssignmentContents(string subject, int num, string season, int year, string category, string asgname)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from a1 in db.Assignments
                    join a2 in db.AssignmentCategories
                    on a1.AssignmentCategoryId equals a2.AssignmentCategoryId


                    join c in db.Classes
                    on a2.ClassId equals c.ClassId

                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber

                    where course.SubjectAbbreviation == subject && c.CourseNumber == num && c.SemesterSeason == season && c.SemesterYear == year && a2.Name == category && a1.Name == asgname
                    select a1.Contents;

                // not sure how to return content
                //return Content(query);
                return Content(query.ToArray()[0]);
            
            }

    }


    /// <summary>
    /// This method does NOT return JSON. It returns plain text (containing html).
    /// Use "return Content(...)" to return plain text.
    /// Returns the contents of an assignment submission.
    /// Returns the empty string ("") if there is no submission.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment in the category</param>
    /// <param name="uid">The uid of the student who submitted it</param>
    /// <returns>The submission text</returns>
    public IActionResult GetSubmissionText(string subject, int num, string season, int year, string category, string asgname, string uid)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from s in db.Submission
                    join a1 in db.Assignments
                    on s.AssignmentId equals a1.AssignmentId
                    where a1.Name == asgname && s.UId == uid


                    join a2 in db.AssignmentCategories
                    on a1.AssignmentCategoryId equals a2.AssignmentCategoryId
                    where a2.Name == category

                    join c in db.Classes
                    on a2.ClassId equals c.ClassId
                    where c.SemesterSeason == season && c.SemesterYear == year

                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber
                    where course.CourseNumber == num && course.SubjectAbbreviation == subject

                    select s.Contents;

                if (query.Count() == 0) return Content("");
                // not sure how to return content
                return Content(query.First());
            }

    }


    /// <summary>
    /// Gets information about a user as a single JSON object.
    /// The object should have the following fields:
    /// "fname": the user's first name
    /// "lname": the user's last name
    /// "uid": the user's uid
    /// "department": (professors and students only) the name (such as "Computer Science") of the department for the user. 
    ///               If the user is a Professor, this is the department they work in.
    ///               If the user is a Student, this is the department they major in.    
    ///               If the user is an Administrator, this field is not present in the returned JSON
    /// </summary>
    /// <param name="uid">The ID of the user</param>
    /// <returns>
    /// The user JSON object 
    /// or an object containing {success: false} if the user doesn't exist
    /// </returns>
    public IActionResult GetUser(string uid)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query1 =
                     from c1 in db.Professors
                     join d in db.Departments
                     on c1.SubjectAbbreviation equals d.SubjectAbbreviation

                     where c1.UId == uid
                     select new
                     {
                         fname = c1.FirstName,
                         lname = c1.LastName,
                         uid = c1.UId,
                         department = d.Name
                     };

                if (query1 != null) return Json(query1);

                var query2 =
                     from c1 in db.Students
                     join d in db.Departments
                     on c1.SubjectAbbreviation equals d.SubjectAbbreviation

                     where c1.UId == uid
                     select new
                     {
                         fname = c1.FirstName,
                         lname = c1.LastName,
                         uid = c1.UId,
                         department = d.Name
                     };

                if (query2 != null) return Json(query2);

                var query3 =
                     from c1 in db.Administrators

                     where c1.UId == uid
                     select new
                     {
                         fname = c1.FirstName,
                         lname = c1.LastName,
                         uid = c1.UId,

                     };

                if (query3.Count() != 0) return Json(query3);

                return Json(new { success = false });
            }

    }


    /*******End code to modify********/

  }
}