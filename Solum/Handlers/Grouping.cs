﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Solum.Handlers
{
	public class Grouping<K, T> : ObservableCollection<T>
	{
		public K Key { get; private set; }
		public Grouping (K key, IEnumerable<T> items)
		{
			Key = key;
			foreach (var item in items)
				this.Items.Add (item);
		}
	}
}

