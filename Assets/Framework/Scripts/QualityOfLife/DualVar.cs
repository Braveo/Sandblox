using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Braveo.QOL {

	//VARIABLES THAT CAN BE TOGGLED ON OR OFF FOR OPTIONAL USE

	[System.Serializable]
	public struct DualVar<T1, T2> {

		public T1 v1;
		public T2 v2;

		public DualVar(T1 val1, T2 val2) {
			this.v1 = val1;
			this.v2 = val2;
		}

	}
}

