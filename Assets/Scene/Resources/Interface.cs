using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus{

	int LoadCapacity{
		get;
		set;
	}
	int MedKit{ get; set; }
	void GetDamage(float Damage);
	void GetHeal (float Heal);
	void GetItem (GameObject item);
	void AddCapacity (int weightItem);
	void minCapacity (int weightItem);

}

public interface IDamageGiver{

	int MaxInMagazine{ get; set; }
	int InMagazine { get; set; }

	void BtnShoot();
	void BtnReload ();


}
