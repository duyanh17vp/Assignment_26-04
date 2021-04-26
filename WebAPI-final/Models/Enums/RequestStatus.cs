using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public enum Status
    {
        waiting = 0,
        Approved = 1,
        Rejected = -1
    }
}
