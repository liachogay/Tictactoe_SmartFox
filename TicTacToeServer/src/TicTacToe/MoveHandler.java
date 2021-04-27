package TicTacToe;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class MoveHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		TicTacToeExtension parent = (TicTacToeExtension) getParentExtension();

		Tile tile;
		arg1.putInt("sender", arg0.getId());
		int x = arg1.getInt("x");
		int y = arg1.getInt("y");

		RoomVariable board = parent.getParentRoom().getVariable("board");
		RoomVariable turn = parent.getParentRoom().getVariable("turn");
		ISFSObject SFSBoard = board.getSFSObjectValue();

		ISFSArray userID = parent.getParentRoom().getVariable("playerID").getSFSArrayValue();

		List<Integer> intList = (List<Integer>) SFSBoard.getIntArray("board");

		if (arg0.getId() == userID.getInt(0)) {
			tile = Tile.Tic;
			arg1.putUtfString("text", "X");
		} else {
			tile = Tile.Tac;
			arg1.putUtfString("text", "O");
		}

		if (intList.get(x * 3 + y) == Tile.Empty.getValue()) {
			intList.set(x * 3 + y, tile.getValue());

			SFSBoard.putIntArray("board", intList);

			List<RoomVariable> lRoomVar = new ArrayList<RoomVariable>();

			board = new SFSRoomVariable("board", SFSBoard);

			Integer IdPlayer1 = userID.getInt(0);

			if (turn.getIntValue() == IdPlayer1) {
				turn = new SFSRoomVariable("turn", userID.getInt(1));
			} else {
				turn = new SFSRoomVariable("turn", userID.getInt(0));
			}

			lRoomVar.add(board);
			lRoomVar.add(turn);

			parent.getParentRoom().setVariables(lRoomVar);

			parent.send("move", arg1, parent.getParentRoom().getPlayersList());

			ISFSObject boardEmpty = new SFSObject();
			if (CheckDraw(intList)) {
				boardEmpty.putBoolArray("boardEmpty", GetCollectionBooleanNull());
				parent.send("getSpotEmpty", boardEmpty, parent.getParentRoom().getUserList());
				parent.send("tie", null, parent.getParentRoom().getUserList());
			} else if (CheckWin(intList)) {
				ISFSObject winner = new SFSObject();
				winner.putInt("winner", arg0.getId());
				boardEmpty.putBoolArray("boardEmpty", GetCollectionBooleanNull());
				parent.send("getSpotEmpty", boardEmpty, parent.getParentRoom().getUserList());
				parent.send("win", winner, parent.getParentRoom().getUserList());
			} else {

				int sizeOfBoard = intList.size();
				Collection<Boolean> boardSpotEmpty = new ArrayList<Boolean>();
				for (int i = 0; i < sizeOfBoard; i++) {
					if (intList.get(i) != 0) {
						boardSpotEmpty.add(false);
					} else {
						boardSpotEmpty.add(true);
					}
				}

				boardEmpty.putBoolArray("boardEmpty", boardSpotEmpty);

				User nextTurn = null;
				List<User> userList = parent.getParentRoom().getUserList();
				for (User i : userList) {
					if (i.getId() == turn.getIntValue()) {
						nextTurn = i;
						break;
					}
				}

				// send to next player all spot is empty
				parent.send("getSpotEmpty", boardEmpty, nextTurn);
			}
		} else {
			trace("The spot is not empty");
		}

	}

	public boolean CheckWin(List<Integer> intList) {
		boolean ret = false;
		if (intList.get(0) == Tile.Empty.getValue() || intList.get(1) == Tile.Empty.getValue()
				|| intList.get(2) == Tile.Empty.getValue() || intList.get(3) == Tile.Empty.getValue()
				|| intList.get(6) == Tile.Empty.getValue()) {
			// enough to no one is win
		} else {
			if (intList.get(0) == intList.get(1) && intList.get(0) == intList.get(2))
				ret = true;
			if (intList.get(0) == intList.get(3) && intList.get(0) == intList.get(6))
				ret = true;
			if (intList.get(0) == intList.get(4) && intList.get(0) == intList.get(8))
				ret = true;
			if (intList.get(1) == intList.get(4) && intList.get(1) == intList.get(7))
				ret = true;
			if (intList.get(2) == intList.get(4) && intList.get(2) == intList.get(6))
				ret = true;
			if (intList.get(2) == intList.get(5) && intList.get(2) == intList.get(8))
				ret = true;
			if (intList.get(3) == intList.get(4) && intList.get(3) == intList.get(5))
				ret = true;
			if (intList.get(6) == intList.get(7) && intList.get(6) == intList.get(8))
				ret = true;
		}
		return ret;
	}

	public boolean CheckDraw(List<Integer> intList) {
		boolean ret = true;
		for (Integer i : intList) {
			if (i != Tile.Empty.getValue()) {
				ret = false;
				break;
			}
		}
		return ret;
	}

	public Collection<Boolean> GetCollectionBooleanNull() {
		Collection<Boolean> ret = new ArrayList<Boolean>();
		for (int i = 0; i < 9; i++) {
			ret.add(false);
		}
		return ret;
	}
}
