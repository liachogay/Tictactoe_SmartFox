package TicTacToe;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.db.DBConfig;
import com.smartfoxserver.v2.db.SFSDBManager;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class DBExtension extends SFSExtension {

	private SFSDBManager DBManager;

	public SFSDBManager getDBMangaer() {
		trace("Called database manager");
		return DBManager;
	}

	@Override
	public void init() {
		// TODO Auto-generated method stub

		DBConfig cfg = new DBConfig();
		cfg.active = true;
		cfg.driverName = "com.microsoft.sqlserver.jdbc.SQLServerDriver";
		cfg.connectionString = "jdbc:sqlserver://localhost:1433;databaseName=TicTacToe;";
		cfg.userName = "admin";
		cfg.password = "1";
		cfg.testSql = "Select * from UserInfo";

		this.addRequestHandler("signup", SignUpHandler.class);
		this.addRequestHandler("test", TestHandler.class);
		this.addRequestHandler("getMoney", GetMoneyHandler.class);
		
		DBManager = new SFSDBManager(cfg);
		trace("Init database done: " + DBManager.getName());
		this.addEventHandler(SFSEventType.USER_JOIN_ZONE, EventJoinZoneHandler.class);
		this.addEventHandler(SFSEventType.USER_LOGIN, LoginEventHandler.class);
		this.addEventHandler(SFSEventType.USER_LEAVE_ROOM, LeaveRoomEventHandler.class);
	}
}
