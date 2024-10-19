using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.Models
{
    public class GPAFilter
    {

            [Key]
            public required int GPAFilterId { get; set; }

            public required float GPAThreshold { get; set; }
        }



        /*public double GPAThreshold { get; set; } // Minimum GPA required*/

    }


