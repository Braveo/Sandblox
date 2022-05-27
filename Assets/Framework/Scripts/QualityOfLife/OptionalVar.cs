using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Braveo.QOL {

	//VARIABLES THAT CAN BE TOGGLED ON OR OFF FOR OPTIONAL USE

	[System.Serializable]
	public struct Optional<T> {

		[SerializeField] bool enabled;
		[SerializeField] T value;

		public Optional(T initial, bool startEnabled = true) {

			enabled = startEnabled;
			value = initial;

		}

		public bool Enabled => enabled;
		public T val {
			get => value;
			set => this.value = value;
		}

		public static implicit operator bool(Optional<T> o) => o.Enabled;
		public static explicit operator T(Optional<T> o) => o.value;

	}
}

