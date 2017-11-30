using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DropTable : ScriptableObject {

	public DropTableElement[] dropTable;

	private int totalWeight;

	void OnEnable(){
		totalWeight = 0;
		if (dropTable.Length > 0) {
			foreach (var item in dropTable) {
				totalWeight += item.weight;
			}
		}
	}

	public GameObject Drop(){
		int roll = Random.Range (1, totalWeight + 1);
		int cursor = 0;
		if (dropTable.Length > 0) {
			
			foreach (var item in dropTable) {
				cursor += item.weight;
				if (cursor >= roll) {
					return item.drop;
				}
			}
		}
		return null;

	}

}
[System.Serializable]
public class DropTableElement{
	public int weight;
	public GameObject drop;
}