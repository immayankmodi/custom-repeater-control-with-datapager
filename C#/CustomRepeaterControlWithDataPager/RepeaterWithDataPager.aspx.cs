﻿using System;
using System.Data;

namespace CustomRepeaterControlWithDataPager {

    public partial class RepeaterWithDataPager : System.Web.UI.Page {

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                LoadResultData();
            }
        }

        private void LoadResultData() {
            DataTable dt = new DataTable();

            //Creating Column
            dt.Columns.Add("SubjectId");
            dt.Columns.Add("SubjectName");
            dt.Columns.Add("Marks");

            //Adding Results in rows
            dt.Rows.Add(1, "Asp.net", 95);
            dt.Rows.Add(2, "C#", 88);
            dt.Rows.Add(3, "Vb.net", 78);
            dt.Rows.Add(4, "HTML", 89);
            dt.Rows.Add(5, "CSS", 90);
            dt.Rows.Add(6, "JavaScript", 85);
            dt.Rows.Add(7, "jQuery", 96);
            dt.Rows.Add(8, "Ajax", 78);
            dt.Rows.Add(9, "SQL", 89);
            dt.Rows.Add(10, "PHP", 78);
            dt.Rows.Add(11, "HTML5", 98);
            dt.Rows.Add(12, "CSS3", 89);

            //Binding Results to repeater control
            rptCustomRepeater.DataSource = dt;
            rptCustomRepeater.DataBind();

            //Make pager invisible when records are less than pagesize
            pgrCustomRepeater.Visible = (pgrCustomRepeater.PageSize < pgrCustomRepeater.TotalRowCount);
        }
    }
}