using System;
using System.Collections.Generic;

namespace Kcg.Models;

public partial class NewsFiles
{
    public Guid NewsFilesId { get; set; }

    public Guid NewsId { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public string Extension { get; set; }
}
