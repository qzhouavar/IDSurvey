using System;
using System.ComponentModel.DataAnnotations;

namespace IDSurvey.Models
{
    public class MailSurveyResult : IComparable, IComparable<MailSurveyResult>, IEquatable<MailSurveyResult>
    {
        [Key]
        public int key { get; set; }

        public string SurveyType { get; set; }

        public int SurveyRound { get; set; }

        public string SurveyQuarter { get; set; }

        public string StateCode { get; set; }

        [Range(1, 5)]
        [Required]
        public int RegionCode { get; set; }

        public string WesId { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime SampleDate { get; set; }

        public string SurveyOutcome { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime InterviewDate { get; set; }

        [Range(1.0, 10.0)]
        public decimal q1 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q2 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q3 { get; set; }

        public string q4 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q5 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q6 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q7 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q8 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q9 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q10 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q11 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q12 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q13 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q14 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q15 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q16 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q17 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q18 { get; set; }

        [Range(1.0, 10.0)]
        public decimal? q19 { get; set; }

        public string q20 { get; set; }

        [Range(1.0, 100.0)]
        public decimal? CoordComp { get; set; }

        [Range(1.0, 100.0)]
        public decimal? OverallComp { get; set; }

        [Range(1.0, 100.0)]
        public decimal? CommunicationComp { get; set; }

        [Range(1.0, 100.0)]
        public decimal? CourtesyComp { get; set; }

        [Range(1.0, 100.0)]
        public decimal? ResponsivenessComp { get; set; }

        [DataType(DataType.Date)]
        public DateTime LoadDate { get; set; }

        public string ContactType { get; set; }
        public string CaseType { get; set; }

        public int CompareTo(object obj)
        {
            return ((IComparable)key).CompareTo(obj);
        }

        public int CompareTo(MailSurveyResult other)
        {
            return key.CompareTo(other.key);
        }

        public bool Equals(MailSurveyResult other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return key.GetHashCode() ^ CoordComp.GetHashCode() ^ OverallComp.GetHashCode() ^ CommunicationComp.GetHashCode() ^ CourtesyComp.GetHashCode() ^ ResponsivenessComp.GetHashCode() ^ InterviewDate.GetHashCode() ^ SampleDate.GetHashCode() ^ LoadDate.GetHashCode() ^ SurveyRound.GetHashCode() ^ RegionCode.GetHashCode() ^ WesId.GetHashCode() ^ SurveyOutcome.GetHashCode() ^ q1.GetHashCode() ^ q2.GetHashCode() ^ q3.GetHashCode() ^ q4.GetHashCode() ^ q5.GetHashCode() ^ q6.GetHashCode() ^ q7.GetHashCode() ^ q8.GetHashCode() ^ q9.GetHashCode() ^ q10.GetHashCode() ^ q11.GetHashCode() ^ q12.GetHashCode() ^ q13.GetHashCode() ^ q14.GetHashCode() ^ q15.GetHashCode() ^ q16.GetHashCode() ^ q17.GetHashCode() ^ q18.GetHashCode() ^ q19.GetHashCode() ^ q20.GetHashCode() ^ SurveyQuarter.GetHashCode() ^ SurveyType.GetHashCode();
        }
    }
}
