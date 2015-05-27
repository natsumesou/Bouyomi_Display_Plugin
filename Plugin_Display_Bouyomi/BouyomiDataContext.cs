namespace Plugin_Display_Bouyomi
{
    class BouyomiDataContext
    {
        public string BouyomiMessage { get; set; }
        public DisplayBouyomiSettings Setting { get; set; }
        public double WindowTop { get; set; }
        public double WindowLeft { get; set; }

        /// <summary>
        /// 処理によっては別の型がDataContextに入るので一度castして自分自身かどうかチェックする
        /// </summary>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        public static bool IsValidInstance(object dataContext)
        {
            try
            {
                string check = ((BouyomiDataContext)dataContext).BouyomiMessage;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
