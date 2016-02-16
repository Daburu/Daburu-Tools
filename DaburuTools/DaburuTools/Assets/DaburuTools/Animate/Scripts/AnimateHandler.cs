/* This is a modified version of the original class.
 * Credit goes to Chong Nian Kai for coming up with the original
 * basic version of this class. I merely took it and enhanced it
 * for our purposes.
 */

using UnityEngine;
using System.Collections;

namespace DaburuTools
{
	namespace Animate
	{
		public class AnimateHandler : MonoBehaviour
		{
			// Static Fields
			private static Animate[] sArrExpandContract;     // sArrExpandContract: All Animate.cs that is in this is going to run an update loop
			private static Animate[] sArrIdle;               // sArrIdle: All Animate.cs that is in this is going to run in an update loop
			private static Animate[] sArrIdleRotation;       // sArrIdleRotation: All Animate.cs that is in this is going to run in an update loop

			// Ediatable Fields
			[Header("Cache Size")]
			[Tooltip("The maximum amount of expand-contract animation it can handle at once")]
			[SerializeField] private int nExpandContractCache = 10;
			[Tooltip("The maximum amount of idling animation it can handle at once")]
			[SerializeField] private int nIdleCache = 10;
			[Tooltip("The maximum amount of idling rotation animation it can handle at once")]
			[SerializeField] private int nIdleRotationCache = 10;

			// Private Functions
			// Awake(): Use this for initialization
			void Awake() 
			{
				// Definition of expand-contract array
				sArrExpandContract = new Animate[nExpandContractCache];
				sArrIdle = new Animate[nIdleCache];
				sArrIdleRotation = new Animate[nIdleRotationCache];
			}

			// Update(): is called once per frame
			void Update () 
			{
				// for: Expand-Contract Checking Sequence
				for (int i = 0; i < sArrExpandContract.Length; i++)
				{
					if (sArrExpandContract[i] != null)
					{
						// if: The current Animate.cs no longer needs to run
						if (!sArrExpandContract[i].__upEC(Time.deltaTime))
							sArrExpandContract[i] = null;

					}
				}

				// for: Idling Checking Sequence
				for (int i = 0; i < sArrIdle.Length; i++)
				{
					if (sArrIdle[i] != null)
					{
						// if: The current Animate.cs no longer needs to run
						if (!sArrIdle[i].__upI(Time.deltaTime))
							sArrIdle[i] = null;
					}
				}

				// for: Idling Rotation Checking Sequence
				for (int i = 0; i < sArrIdleRotation.Length; i++)
				{
					if (sArrIdleRotation[i] != null)
					{
						// if: The current Animate.cs no longer needs to run
						if (!sArrIdleRotation[i].__upIR(Time.deltaTime))
							sArrIdleRotation[i] = null;
					}
				}
			}



			// Public Static Functions
			// ActivateExpandContract(): Pushes an Animate.cs into update sequence
			public static bool ActivateExpandContract(Animate _mAnimate)
			{
				for (int i = 0; i < sArrExpandContract.Length; i++)
				{
					if (sArrExpandContract[i] == null)
					{
						sArrExpandContract[i] = _mAnimate;
						return true;
					}
				}
				Debug.LogWarning("AnimateHandler.ActivateExpandContract(): Cache has reached its maximum limit, consider creating a bigger cache?");
				return false;
			}

			// ActivateIdle(): Pushes an Animate.cs into update sequence
			public static bool ActivateIdle(Animate _mAnimate)
			{
				for (int i = 0; i < sArrIdle.Length; i++)
				{
					if (sArrIdle[i] == null)
					{
						sArrIdle[i] = _mAnimate;
						return true;
					}
				}
				Debug.LogWarning("AnimateHandler.ActivateIdle(): Cache has reached its maximum limit, consider creating a bigger cache?");
				return false;
			}

			// ActivateIdleRotation(): Pushes an Animate.cs into update sequence
			public static bool ActivateIdleRotation(Animate _mAnimate)
			{
				for (int i = 0; i < sArrIdleRotation.Length; i++)
				{
					if (sArrIdleRotation[i] == null)
					{
						sArrIdleRotation[i] = _mAnimate;
						return true;
					}
				}
				Debug.LogWarning("AnimateHandler.ActivateIdleRotation(): Cache has reached its maximum limit, consider creating a bigger cache?");
				return false;
			}
		}
	}
}