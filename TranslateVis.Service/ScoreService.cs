using System;
using System.Collections.Generic;
using System.Text;
using TranslateVis.DAL.DataBase;
using TranslateVis.DAL.DataEntities;
using TranslateVis.DTO;

namespace TranslateVis.Service
{
    public class ScoreService : Repository<UserScore>
    {

        public AttemdanceDetailOutput GetDetail(int id)
        {
            AttemdanceDetailOutput detailOutput = new AttemdanceDetailOutput();
            var userScore = base.GetById(id);
            if (userScore == null) return detailOutput;
            detailOutput = new AttemdanceDetailOutput
            {
                Score = userScore.Score,
                AScore = userScore.AScore,
                BScore = userScore.BScore,
                CScore = userScore.CScore,
                DScore = userScore.DScore,
                AttendanceDate = userScore.AttendanceDate,
                AttendanceType = userScore.AttendanceType,
                Id = userScore.Id,
                Remark = userScore.Remark ?? string.Empty,
                UserId = userScore.UserId
            };
            var userInfo = base.Context.Queryable<UserInfo>().First(x => x.Id == userScore.UserId);
            detailOutput.LinkMan = userInfo != null ? userInfo.LinkMan : "";
            return detailOutput;
        }
    }
}
