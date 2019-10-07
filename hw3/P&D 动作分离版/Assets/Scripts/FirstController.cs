using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {

	readonly Vector3 water_pos = new Vector3(0,0.5F,0);

	UserGUI userGUI;

	public SSDirector director;
	public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	private MyCharacterController[] characters;
	private ActionManager MyActionManager;
	private Referee MyReferee;


	void Awake() {
		director = SSDirector.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		characters = new MyCharacterController[6];
		MyActionManager = gameObject.AddComponent<ActionManager>() as ActionManager;
		MyReferee = new Referee(boat);
		loadResources ();
	}

	public void loadResources() {
		// load water
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

		// load coast and boat
		fromCoast = new CoastController ("from");
		toCoast = new CoastController ("to");
		boat = new BoatController ();

		// load characters
		for (int i = 0; i < 3; i++) {
			MyCharacterController newCharacter = new MyCharacterController ("priest");
			newCharacter.setName("priest" + i);
			newCharacter.setPosition (fromCoast.getEmptyPosition ());
			newCharacter.getOnCoast (fromCoast);
			fromCoast.getOnCoast (newCharacter);

			characters [i] = newCharacter;
		}

		for (int i = 0; i < 3; i++) {
			MyCharacterController newCharacter = new MyCharacterController ("devil");
			newCharacter.setName("devil" + i);
			newCharacter.setPosition (fromCoast.getEmptyPosition ());
			newCharacter.getOnCoast (fromCoast);
			fromCoast.getOnCoast (newCharacter);

			characters [i+3] = newCharacter;
		}
	}

	public void moveBoat() {
		if (boat.isEmpty () || userGUI.status!=0)
			return;
		//boat.Move ();
		MyActionManager.MoveBoat(boat);
		userGUI.status = MyReferee.Check (fromCoast, toCoast, boat);
	}

	public void characterIsClicked(MyCharacterController characterCtrl) {
		Debug.Log("here!");
		if (userGUI.status!=0)
			return;
		
		
		if (characterCtrl.isOnBoat ()) {
			Debug.Log("here2!");
			CoastController whichCoast;
			if (boat.get_to_or_from () == -1) { // 左(to) -1; 右(from) 1
				whichCoast = toCoast;
			} else {
				whichCoast = fromCoast;
			}

			boat.GetOffBoat (characterCtrl.getName());
			//characterCtrl.moveToPosition (whichCoast.getEmptyPosition ());
			MyActionManager.MoveCharacter(characterCtrl, whichCoast.getEmptyPosition());
			characterCtrl.getOnCoast (whichCoast);
			whichCoast.getOnCoast (characterCtrl);

		} else {									// character on coast
			
			CoastController whichCoast = characterCtrl.getCoastController ();

			if (boat.getEmptyIndex () == -1) {		// boat is full
				return;
			}

			if (whichCoast.get_to_or_from () != boat.get_to_or_from ())	// boat is not on the side of character
				return;
			Debug.Log("here3!");
			whichCoast.getOffCoast(characterCtrl.getName());
			//characterCtrl.moveToPosition (boat.getEmptyPosition());
			MyActionManager.MoveCharacter(characterCtrl, boat.getEmptyPosition());
			characterCtrl.getOnBoat (boat);
			boat.getOnBoat (characterCtrl);
		}
		userGUI.status = MyReferee.Check(fromCoast, toCoast, boat);
	}

	/*
	int check_game_over() {	// 0->not finish, 1->lose, 2->win
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = fromCoast.getCharacterNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getCharacterNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)		// win
			return 2;

		int[] boatCount = boat.getCharacterNum ();
		if (boat.get_to_or_from () == -1) {	// boat at toCoast
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} else {	// boat at fromCoast
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}
		if (from_priest < from_devil && from_priest > 0) {		// lose
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0) {
			return 1;
		}
		return 0;			// not finish
	}
	 */
	

	public void restart() {
		boat.reset();
		if(boat.get_to_or_from() == -1)
			MyActionManager.MoveBoat(boat);
		fromCoast.reset ();
		toCoast.reset ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].reset ();
		}
	}
}
