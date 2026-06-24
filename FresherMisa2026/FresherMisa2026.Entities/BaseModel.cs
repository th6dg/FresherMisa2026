using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FresherMisa2026.Entities
{
    /// <summary>
    /// Dán nhãn IRequired lên Property 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IRequired : Attribute
    {

    }

    public class BaseModel
    {
        /// <summary>
        /// Người tạo
        /// </summary>
        [NotMapped]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [NotMapped]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Trạng thái thêm sửa xóa, không lưu database
        /// </summary>
        [NotMapped]
        public ModelSate State { get; set; }

        /// <summary>
        /// Có xóa mềm hay không
        /// </summary>
        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}
