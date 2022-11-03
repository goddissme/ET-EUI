using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_LoginAccount a2c_Login;
            Session session = null;

            try
            {
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                a2c_Login = (A2C_LoginAccount)await session.Call(new C2A_LoginAccount() {AccountName = account, Password = password});
            }
            catch (Exception e)
            {
                session?.Dispose();
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2c_Login.Error != ErrorCode.ERR_Success)
            {
                session?.Dispose();
                return a2c_Login.Error;
            }

            zoneScene.AddComponent<SessionComponent>().Session = session;

            return ErrorCode.ERR_Success;
        } 
    }
}