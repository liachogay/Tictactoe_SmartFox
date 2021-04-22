package TicTacToe;

public final class PlayerTurnManager {
	private static final PlayerTurnManager _instance;

	// Static block initialization for exception handling
	static {
		try {
			_instance = new PlayerTurnManager();
		} catch (Exception e) {
			throw new RuntimeException("Exception occured in creating singleton instance");
		}
	}

	public static PlayerTurnManager Instance() {
		return _instance;
	}

	private final int[] _players;
	private final String[] _namePlayers;

	private int _turn = 0;

	private PlayerTurnManager() {
		_players = new int[2];
		_namePlayers = new String[2];
	}

	public void Reset() {
	}

	public int[] getPlayers() {
		return _players;
	}

	public void setPlayers(int indexPlayer, int idPlayer) {
		_players[indexPlayer] = idPlayer;
	}

	public int getPlayers(int indexPlayer) {
		return _players[indexPlayer];
	}

	public String[] getNamePlayers() {
		return _namePlayers;
	}

	public String getNamePlayers(int indexPlayer) {
		return _namePlayers[indexPlayer];
	}

	public void setNamePlayers(int indexPlayer, String namePlayer) {
		_namePlayers[indexPlayer] = namePlayer;
	}

	public int getTurn() {
		return _turn;
	}

	public void setTurn(int _turn) {
		this._turn = _turn;
	}

	public void nextTurn() {
		for (int i = 0; i < _players.length; i++) {
			if (_turn == _players[i]) {
				_turn = _players[(i + 1) % _players.length];
				break;
			}
		}
	}
}
