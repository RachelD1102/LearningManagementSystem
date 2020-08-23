using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
  [Authorize(Roles = "Professor")]
  public class ProfessorController : CommonController
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Students(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult Class(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult Categories(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      return View();
    }

    public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      return View();
    }

    public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      return View();
    }

    public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      ViewData["uid"] = uid;
      return View();
    }

    /*******Begin code to modify********/


    /// <summary>
    /// Returns a JSON array of all the students in a class.
    /// Each object in the array should have the following fields:
    /// "fname" - first name
    /// "lname" - last name
    /// "uid" - user ID
    /// "dob" - date of birth
    /// "grade" - the student's grade in this class
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from c in db.Classes
                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber



                    where course.SubjectAbbreviation == subject && course.CourseNumber == num && c.SemesterSeason == season && c.SemesterYear == year
                    select c.ClassId;


                var query1 =
                    from se in db.StudentEnrollment
                    join s in db.Students
                    on se.UId equals s.UId

                    where se.ClassId == query.ToArray()[0]
                    select new
                    {
                        fname = s.FirstName,
                        lname = s.LastName,
                        uid = s.UId,
                        dob = s.DateOfBirth,
                        grade = se.Grade

                    };
                return Json(query1.ToArray());
            }

    }



    /// <summary>
    /// Returns a JSON array with all the assignments in an assignment category for a class.
    /// If the "category" parameter is null, return all assignments in the class.
    /// Each object in the array should have the following fields:
    /// "aname" - The assignment name
    /// "cname" - The assignment category name.
    /// "due" - The due DateTime
    /// "submissions" - The number of submissions to the assignment
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class, 
    /// or null to return assignments from all categories</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {

                JsonResult result;
                var query1 = 
                    from c in db.Courses
                    join cla in db.Classes
                    on c.CourseNumber equals cla.CourseNumber
                    where c.SubjectAbbreviation == subject && c.CourseNumber == num && cla.SemesterSeason == season && cla.SemesterYear == year
                    join categories in db.AssignmentCategories
                    on cla.ClassId equals categories.ClassId
                    join a in db.Assignments
                    on categories.AssignmentCategoryId equals a.AssignmentCategoryId
                    select new { a, categories };

                if (category != null)
                {

                    var query2 = 
                        from q in query1
                        where q.categories.Name == category
                        select new
                        {
                           aname = q.a.Name,
                           cname = q.categories.Name,
                           due = q.a.DueDate,
                           submissions = (from s in db.Submission where s.AssignmentId == q.a.AssignmentId select s).Count()
                        };

                    result = Json(query2.ToArray());

                }
                else
                {

                    var query2 = 
                        from q in query1
                        select new
                        {
                          aname = q.a.Name,
                          cname = q.categories.Name,
                          due = q.a.DueDate,
                          submissions = (from s in db.Submission where s.AssignmentId == q.a.AssignmentId select s).Count()
                        };

                    result = Json(query2.ToArray());
                }

                return result;
            }

    }


    /// <summary>
    /// Returns a JSON array of the assignment categories for a certain class.
    /// Each object in the array should have the folling fields:
    /// "name" - The category name
    /// "weight" - The category weight
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from c in db.Classes
                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber

                    where course.SubjectAbbreviation == subject && course.CourseNumber == num && c.SemesterSeason == season && c.SemesterYear == year
                    select c.ClassId;


                var query1 =


                    from ac in db.AssignmentCategories

                    where ac.ClassId == query.ToArray()[0]
                    select new
                    {
                        name = ac.Name,
                        weight = ac.GradingWeight

                    };
                db.SaveChanges();//I am not sure here
                return Json(query1.ToArray());
            }

    }

    /// <summary>
    /// Creates a new assignment category for the specified class.
    /// If a category of the given class with the given name already exists, return success = false.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The new category name</param>
    /// <param name="catweight">The new category weight</param>
    /// <returns>A JSON object containing {success = true/false} </returns>
    public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
    {

            var chars = "0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var acID = new String(stringChars);

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from c in db.Classes
                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber

                    where course.SubjectAbbreviation == subject && course.CourseNumber == num && c.SemesterSeason == season && c.SemesterYear == year
                    select c.ClassId;

                var query1 =
                    from ac in db.AssignmentCategories
                    where ac.Name == category && ac.ClassId == query.ToArray()[0]
                    select ac;
                if (query1.Count() != 0) return Json(new { success = false });

                Models.LMSModels.AssignmentCategories ac1 = new Models.LMSModels.AssignmentCategories();
                ac1.ClassId = query.ToArray()[0];
                ac1.AssignmentCategoryId = acID;
                ac1.GradingWeight = catweight;
                ac1.Name = category;
                db.AssignmentCategories.Add(ac1);
                db.SaveChanges();


                // update all students' grades for this class

                var findAllStudents =
                    from se in db.StudentEnrollment
                    where se.ClassId == query.ToArray()[0]
                    select se.UId;

                foreach (var x in findAllStudents)
                {
                    updateStudentGradeForClass(query.ToArray()[0], x);
                }
                return Json(new { success = true });
            }

    }

    /// <summary>
    /// Creates a new assignment for the given class and category.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The new assignment name</param>
    /// <param name="asgpoints">The max point value for the new assignment</param>
    /// <param name="asgdue">The due DateTime for the new assignment</param>
    /// <param name="asgcontents">The contents of the new assignment</param>
    /// <returns>A JSON object containing success = true/false</returns>
    public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
    {

            var chars = "0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var AssignmentID = new String(stringChars);

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from c in db.Classes
                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber

                    where course.SubjectAbbreviation == subject && course.CourseNumber == num && c.SemesterSeason == season && c.SemesterYear == year
                    select c.ClassId;

                var query1 =
                    from ac in db.AssignmentCategories
                    where ac.Name == category && ac.ClassId == query.ToArray()[0]
                    select ac.AssignmentCategoryId;

                var query2 =
                    from a in db.Assignments
                    where a.AssignmentCategoryId == query1.ToArray()[0] && a.Name == asgname
                    select a;

                if (query2.Count() != 0) return Json(new { success = false });

                Models.LMSModels.Assignments a1 = new Models.LMSModels.Assignments();
                a1.AssignmentId = AssignmentID;
                a1.DueDate = asgdue;
                a1.Name = asgname;
                a1.Contents = asgcontents;
                a1.MaxPointValue = asgpoints;
                a1.AssignmentCategoryId = query1.ToArray()[0];
                db.Assignments.Add(a1);
                db.SaveChanges();

                var allStudents =
                    from se in db.StudentEnrollment
                    where se.ClassId == query.ToArray()[0]
                    select se;

                foreach (var s in allStudents) {

                    updateStudentGradeForClass(s.ClassId, s.UId);
                }

                return Json(new { success = true });
            }

    }


    /// <summary>
    /// Gets a JSON array of all the submissions to a certain assignment.
    /// Each object in the array should have the following fields:
    /// "fname" - first name
    /// "lname" - last name
    /// "uid" - user ID
    /// "time" - DateTime of the submission
    /// "score" - The score given to the submission
    /// 
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from c in db.Classes
                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber

                    where course.SubjectAbbreviation == subject && course.CourseNumber == num && c.SemesterSeason == season && c.SemesterYear == year
                    select c.ClassId;

                var query1 =
                    from ac in db.AssignmentCategories
                    where ac.Name == category && ac.ClassId == query.ToArray()[0]
                    select ac.AssignmentCategoryId;

                var query2 =
                    from a in db.Assignments
                    where a.AssignmentCategoryId == query1.ToArray()[0] && a.Name == asgname
                    select a.AssignmentId;

                var query3 =
                    from s in db.Submission
                    join student in db.Students
                    on s.UId equals student.UId

                    where s.AssignmentId == query2.ToArray()[0]
                    select new
                    {
                        fname = student.FirstName,
                        lname = student.LastName,
                        uid = student.UId,
                        time = s.SubmitTime,
                        score = s.Score
                    };


                return Json(query3.ToArray());
            }

    }


    /// <summary>
    /// Set the score of an assignment submission
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment</param>
    /// <param name="uid">The uid of the student who's submission is being graded</param>
    /// <param name="score">The new score for the submission</param>
    /// <returns>A JSON object containing success = true/false</returns>
    public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from c in db.Classes
                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber

                    where course.SubjectAbbreviation == subject && course.CourseNumber == num && c.SemesterSeason == season && c.SemesterYear == year
                    select c.ClassId;

                var query1 =
                    from ac in db.AssignmentCategories
                    where ac.Name == category && ac.ClassId == query.ToArray()[0]
                    select ac.AssignmentCategoryId;

                var query2 =
                    from a in db.Assignments
                    where a.AssignmentCategoryId == query1.ToArray()[0] && a.Name == asgname
                    select a.AssignmentId;

                var query3 =
                    from s in db.Submission
                    where s.AssignmentId == query2.ToArray()[0] && s.UId == uid
                    select s;

                if (query3.Count() == 0) return Json(new { success = false });

                query3.ToArray()[0].Score = score;
                db.SaveChanges();

                // change studuent's grade for this class automatically
                updateStudentGradeForClass(query.ToArray()[0], uid);
               


                return Json(new { success = true });
            }

    }


    /// <summary>
    /// Returns a JSON array of the classes taught by the specified professor
    /// Each object in the array should have the following fields:
    /// "subject" - The subject abbreviation of the class (such as "CS")
    /// "number" - The course number (such as 5530)
    /// "name" - The course name
    /// "season" - The season part of the semester in which the class is taught
    /// "year" - The year part of the semester in which the class is taught
    /// </summary>
    /// <param name="uid">The professor's uid</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetMyClasses(string uid)
    {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var query =
                    from c in db.Classes
                    join course in db.Courses
                    on c.CourseNumber equals course.CourseNumber

                    where c.ProfessorId == uid
                    select new
                    {
                        subject = course.SubjectAbbreviation,
                        number = course.CourseNumber,
                        name = course.Name,
                        season = c.SemesterSeason,
                        year = c.SemesterYear
                    };

                return Json(query.ToArray());
            }

    }

        public void updateStudentGradeForClass(String classId, String uid)
        {

            using (Models.LMSModels.Team13LMSContext db = new Models.LMSModels.Team13LMSContext())
            {
                var findAllAssignmentCategories =
                    from ac in db.AssignmentCategories
                    where ac.ClassId == classId
                    select ac;

                int assignmentCategoriesWeightSum = 0;
                int maxPointsForAllAssignments = 0;
                int pointsForAllAssignments = 0;
                double curGrade = 0.0;
                foreach (var ac in findAllAssignmentCategories)
                {
                    int totalMaxPoints = 0;
                    int totalPoints = 0;
                    var allAssignments =
                        from a in db.Assignments
                        where a.AssignmentCategoryId == ac.AssignmentCategoryId
                        select a;




                    // do not have any submitted assignment for this student in this assignmentCategory
                    // then skip this loop
                    if (allAssignments.Count() == 0) continue;

                    assignmentCategoriesWeightSum += (int)ac.GradingWeight;


                    foreach (var x in allAssignments)
                    {
                        totalMaxPoints += (int)x.MaxPointValue;
                        maxPointsForAllAssignments += (int)x.MaxPointValue;
                        var submission =
                            from s in db.Submission
                            where s.AssignmentId == x.AssignmentId && s.UId == uid
                            select s;
                        if (submission.Count() == 0)
                        {
                            totalPoints += 0;
                            pointsForAllAssignments += 0;
                        }
                        else
                        {
                            totalPoints += (int)submission.ToArray()[0].Score;
                            pointsForAllAssignments += (int)submission.ToArray()[0].Score;
                        }
                    }

                    // calculate percentage for this assignmentCategory
                    double percentage = (double) totalPoints / (double) totalMaxPoints;
                    curGrade += percentage * (double)ac.GradingWeight;
                }
                double scalingFactor = (double) 100 / (double) assignmentCategoriesWeightSum;
                curGrade *= scalingFactor;
                //90 > X _ 87 B+ 80 > X _ 77 C+ 70 > X _ 67 D+
                //100 _ X _ 93 A 87 > X _ 83 B 77 > X _ 73 C 67 > X _ 63 D 60 > X _ 0 E
                //93 > X _ 90 A - 83 > X _ 80 B - 73 > X _ 70 C - 63 > X _ 60 D -

                String curGradeLetter = "";
                if (pointsForAllAssignments >= maxPointsForAllAssignments) curGradeLetter = "A";
                if (curGrade >= 93) curGradeLetter = "A";
                else if (curGrade >= 90 && curGrade < 93) curGradeLetter = "A-";
                else if (curGrade >= 87 && curGrade < 90) curGradeLetter = "B+";
                else if (curGrade >= 83 && curGrade < 87) curGradeLetter = "B";
                else if (curGrade >= 80 && curGrade < 83) curGradeLetter = "B-";
                else if (curGrade >= 77 && curGrade < 80) curGradeLetter = "C+";
                else if (curGrade >= 73 && curGrade < 77) curGradeLetter = "C";
                else if (curGrade >= 70 && curGrade < 73) curGradeLetter = "C-";
                else if (curGrade >= 67 && curGrade < 70) curGradeLetter = "D+";
                else if (curGrade >= 63 && curGrade < 67) curGradeLetter = "D";
                else if (curGrade >= 60 && curGrade < 63) curGradeLetter = "D-";
                else curGradeLetter = "E";

                


                var findEnrollment =
                    from se in db.StudentEnrollment
                    where se.UId == uid && se.ClassId == classId
                    select se;


                findEnrollment.ToArray()[0].Grade = curGradeLetter;

                db.SaveChanges();
            }

        }


        /*******End code to modify********/

  }
}