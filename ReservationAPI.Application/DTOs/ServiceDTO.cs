﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationAPI.Application.DTOs
{
    public class Service
    {
		private int _id;

		public int id
		{
			get { return _id; }
			set { _id = value; }
		}
		private string _name;

		public string name
		{
			get { return _name; }
			set { _name = value; }
		}

	}
}
