using UnityEngine;
using System.Collections;

public interface IAttackable {
	 void Attacked(PlayerChar attacker, int wound);
}
