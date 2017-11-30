using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/ObjectBehavioursList")]
public class ObjectBehavioursList : ObjectBehaviour {

	public ObjectBehaviour[] objectBehaviours;


	#region implemented abstract members of ObjectBehaviour
	public override void Execute (GameObject obj)
	{
		if (objectBehaviours.Length > 0) {
			foreach (var item in objectBehaviours) {
				item.Execute (obj);
			}
		}
	}
	#endregion


}
