﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backEnd.Models;

namespace backEnd.Managers
{
    public class TagsManager
    {
        public static Tags UserPostsCountPlus(PaintStoreContext db, int id)
        {
            var countTool = db.Tags.Where(x => x.Id == id).First();
            countTool.Count += 1;
            return countTool;
        }
        public static Tags UserPostsCountMinus(PaintStoreContext db, int id)
        {
            var countTool = db.Tags.Where(x => x.Id == id).First();
            countTool.Count -= 1;
            return countTool;
        }
    }
}
