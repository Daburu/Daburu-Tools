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
			private static Animate[] s_arrayExpandContract;     // s_arrayExpandContract: All Animate.cs that is in this is going to run an update loop
			private static Animate[] s_arrayIdle;               // s_arrayIdle: All Animate.cs that is in this is going to run in an update loop
			private static Animate[] s_arrayIdleRotation;       // s_arrayIdleRotation: All Animate.cs that is in this is going to run in an update loop

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
				s_arrayExpandContract = new Animate[nExpandContractCache];
				s_arrayIdle = new Animate[nIdleCache];
				s_arrayIdleRotation = new Animate[nIdleRotationCache];
			}

			// Update(): is called once per frame
			void Update () 
			{
				// for: Expand-Contract Checking Sequence
				for (int i = 0; i < s_arrayExpandContract.Length; i++)
				{
					if (s_arrayExpandContract[i] != null)
					{
						// if: The current Animate.cs no longer needs to run
						if (!s_arrayExpandContract[i].__upEC(Time.deltaTime))
							s_arrayExpandContract[i] = null;

					}
				}

				// for: Idling Checking Sequence
				for (int i = 0; i < s_arrayIdle.Length; i++)
				{
					if (s_arrayIdle[i] != null)
					{
						// if: The current Animate.cs no longer needs to run
						if (!s_arrayIdle[i].__upI(Time.deltaTime))
							s_arrayIdle[i] = null;
					}
				}

				// for: Idling Rotation Checking Sequence
				for (int i = 0; i < s_arrayIdleRotation.Length; i++)
				{
					if (s_arrayIdleRotation[i] != null)
					{
						// if: The current Animate.cs no longer needs to run
						if (!s_arrayIdleRotation[i].__upIR(Time.deltaTime))
							s_arrayIdleRotation[i] = null;
					}
				}
			}

			// Public Static Functions
			// ActivateExpandContract(): Pushes an Animate.cs into update sequence
			public static bool ActivateExpandContract(Animate _mAnimate)
			{
				for (int i = 0; i < s_arrayExpandContract.Length; i++)
				{
					if (s_arrayExpandContract[i] == null)
					{
						s_arrayExpandContract[i] = _mAnimate;
						return true;
					}
				}
				Debug.LogWarning("AnimateHandler.ActivateExpandContract(): Cache have reached its maximum limit, consider creating a bigger cache?");
				return false;
			}

			// ActivateIdle(): Pushes an Animate.cs into update sequence
			public static bool ActivateIdle(Animate _mAnimate)
			{
				for (int i = 0; i < s_arrayIdle.Length; i++)
				{
					if (s_arrayIdle[i] == null)
					{
						s_arrayIdle[i] = _mAnimate;
						return true;
					}
				}
				Debug.LogWarning("AnimateHandler.ActivateIdle(): Cache have reached its maximum limit, consider creating a bigger cache?");
				return false;
			}

			// ActivateIdleRotation(): Pushes an Animate.cs into update sequence
			public static bool ActivateIdleRotation(Animate _mAnimate)
			{
				for (int i = 0; i < s_arrayIdleRotation.Length; i++)
				{
					if (s_arrayIdleRotation[i] == null)
					{
						s_arrayIdleRotation[i] = _mAnimate;
						return true;
					}
				}
				Debug.LogWarning("AnimateHandler.ActivateIdleRotation(): Cache have reached its maximum limit, consider creating a bigger cache?");
				return false;
			}
		}
	}
}