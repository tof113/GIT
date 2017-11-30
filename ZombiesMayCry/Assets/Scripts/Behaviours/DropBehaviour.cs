using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Behaviours/DropBehaviour")]
public class DropBehaviour : ObjectBehaviour {

	public DropTable dropTable;
	#region implemented abstract members of ObjectBehaviour
	public override void Execute (GameObject obj)
	{
		if (dropTable) {
			GameObject drop = dropTable.Drop();
			if (drop) {
				
				Poolable poolable = drop.GetComponent<Poolable> ();
				if (poolable) {
					drop = poolable.GetInstance ();


				} else {
					drop = Instantiate (drop);
				}
				drop.transform.position = obj.transform.position;

			}
		}
	}
	#endregion


}
