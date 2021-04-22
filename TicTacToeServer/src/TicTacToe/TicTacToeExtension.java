package TicTacToe;

import com.smartfoxserver.v2.extensions.SFSExtension;

public class TicTacToeExtension extends SFSExtension {
	@Override
	public void init() {
		this.addRequestHandler("move", MoveHandler.class);
		this.addRequestHandler("ready", ReadyHandler.class);
		this.addRequestHandler("getSpotEmpty", GetSpotEmptyHandler.class);
	}

}
