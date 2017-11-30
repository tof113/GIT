using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/PoolBehaviour")]
public class PoolBehaviour : ObjectBehaviour {

	public ObjectBehaviour fallBackBehaviour;

	#region implemented abstract members of ObjectBehaviour

	public override void Execute (GameObject obj)
	{
		Poolable poolable = obj.GetComponent<Poolable>();

		if (!poolable) {
			if (fallBackBehaviour) {
				
				fallBackBehaviour.Execute (obj);
			}
		} else {
			poolable.TryPool ();
		}
	}

	#endregion



}
