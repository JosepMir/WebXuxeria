using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebXuxeria.Models
{
    public class MainModel
    {
        [Display(Name = "MainForm")]
        public List<Word> Words { get; set; }
        public int? CollectionId { get; set; }
        public string CollectionName { get; set; }

        public Guid CollectionPictureId { get; set; }

        public bool Ordenades { get; set; }
     
    }
}
