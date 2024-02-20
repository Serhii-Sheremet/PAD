using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD
{
    public class Day
    {
        public DateTime Date { get; set; }
        public bool IsDayOfMonth { get; set; }

        // Person specific data
        public DateTime? SunRise { get; set; }
        public DateTime? SunSet { get; set; }

        //Masa / Shunya lists
        public List<Calendar> MasaDayList { get; set; }
        public List<Calendar> ShunyaNakshatraDayList { get; set; }
        public List<Calendar> ShunyaTithiDayList { get; set; }

        //Calendars from natal moon lists
        public List<Calendar> MoonZodiakDayList { get; set; }
        public List<Calendar> MoonZodiakRetroDayList { get; set; }
        public List<Calendar> MoonNakshatraDayList { get; set; }
        public List<Calendar> MoonPadaDayList { get; set; }
        public List<Calendar> MoonTaraBalaDayList { get; set; }

        public List<Calendar> SunZodiakDayList { get; set; }
        public List<Calendar> SunZodiakRetroDayList { get; set; }
        public List<Calendar> SunNakshatraDayList { get; set; }
        public List<Calendar> SunPadaDayList { get; set; }
        public List<Calendar> SunTaraBalaDayList { get; set; }

        public List<Calendar> MercuryZodiakDayList { get; set; }
        public List<Calendar> MercuryZodiakRetroDayList { get; set; }
        public List<Calendar> MercuryNakshatraDayList { get; set; }
        public List<Calendar> MercuryPadaDayList { get; set; }
        public List<Calendar> MercuryTaraBalaDayList { get; set; }

        public List<Calendar> VenusZodiakDayList { get; set; }
        public List<Calendar> VenusZodiakRetroDayList { get; set; }
        public List<Calendar> VenusNakshatraDayList { get; set; }
        public List<Calendar> VenusPadaDayList { get; set; }
        public List<Calendar> VenusTaraBalaDayList { get; set; }

        public List<Calendar> MarsZodiakDayList { get; set; }
        public List<Calendar> MarsZodiakRetroDayList { get; set; }
        public List<Calendar> MarsNakshatraDayList { get; set; }
        public List<Calendar> MarsPadaDayList { get; set; }
        public List<Calendar> MarsTaraBalaDayList { get; set; }

        public List<Calendar> JupiterZodiakDayList { get; set; }
        public List<Calendar> JupiterZodiakRetroDayList { get; set; }
        public List<Calendar> JupiterNakshatraDayList { get; set; }
        public List<Calendar> JupiterPadaDayList { get; set; }
        public List<Calendar> JupiterTaraBalaDayList { get; set; }

        public List<Calendar> SaturnZodiakDayList { get; set; }
        public List<Calendar> SaturnZodiakRetroDayList { get; set; }
        public List<Calendar> SaturnNakshatraDayList { get; set; }
        public List<Calendar> SaturnPadaDayList { get; set; }
        public List<Calendar> SaturnTaraBalaDayList { get; set; }

        public List<Calendar> RahuMeanZodiakDayList { get; set; }
        public List<Calendar> RahuMeanZodiakRetroDayList { get; set; }
        public List<Calendar> RahuMeanNakshatraDayList { get; set; }
        public List<Calendar> RahuMeanPadaDayList { get; set; }
        public List<Calendar> RahuMeanTaraBalaDayList { get; set; }

        public List<Calendar> KetuMeanZodiakDayList { get; set; }
        public List<Calendar> KetuMeanZodiakRetroDayList { get; set; }
        public List<Calendar> KetuMeanNakshatraDayList { get; set; }
        public List<Calendar> KetuMeanPadaDayList { get; set; }
        public List<Calendar> KetuMeanTaraBalaDayList { get; set; }

        public List<Calendar> RahuTrueZodiakDayList { get; set; }
        public List<Calendar> RahuTrueZodiakRetroDayList { get; set; }
        public List<Calendar> RahuTrueNakshatraDayList { get; set; }
        public List<Calendar> RahuTruePadaDayList { get; set; }
        public List<Calendar> RahuTrueTaraBalaDayList { get; set; }

        public List<Calendar> KetuTrueZodiakDayList { get; set; }
        public List<Calendar> KetuTrueZodiakRetroDayList { get; set; }
        public List<Calendar> KetuTrueNakshatraDayList { get; set; }
        public List<Calendar> KetuTruePadaDayList { get; set; }
        public List<Calendar> KetuTrueTaraBalaDayList { get; set; }

        //Calendars from lagna lists
        public List<Calendar> MoonZodiakLagnaDayList { get; set; }
        public List<Calendar> MoonZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> SunZodiakLagnaDayList { get; set; }
        public List<Calendar> SunZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> MercuryZodiakLagnaDayList { get; set; }
        public List<Calendar> MercuryZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> VenusZodiakLagnaDayList { get; set; }
        public List<Calendar> VenusZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> MarsZodiakLagnaDayList { get; set; }
        public List<Calendar> MarsZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> JupiterZodiakLagnaDayList { get; set; }
        public List<Calendar> JupiterZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> SaturnZodiakLagnaDayList { get; set; }
        public List<Calendar> SaturnZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> RahuMeanZodiakLagnaDayList { get; set; }
        public List<Calendar> RahuMeanZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> KetuMeanZodiakLagnaDayList { get; set; }
        public List<Calendar> KetuMeanZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> RahuTrueZodiakLagnaDayList { get; set; }
        public List<Calendar> RahuTrueZodiakRetroLagnaDayList { get; set; }

        public List<Calendar> KetuTrueZodiakLagnaDayList { get; set; }
        public List<Calendar> KetuTrueZodiakRetroLagnaDayList { get; set; }

        // Other calendars
        public List<Calendar> NakshatraDayList { get; set; }
        public List<Calendar> TaraBalaDayList { get; set; }
        public List<Calendar> TithiDayList { get; set; }
        public List<Calendar> KaranaDayList { get; set; }
        public List<Calendar> ChandraBalaDayList { get; set; }
        public List<Calendar> NityaJogaDayList { get; set; }
        public List<Calendar> EclipseDayList { get; set; }

        public List<Calendar> DwipushkarJogaDayList { get; set; }
        public List<Calendar> TripushkarJogaDayList { get; set; }
        public List<Calendar> AmritaSiddhaJogaDayList { get; set; }
        public List<Calendar> SarvarthaSiddhaJogaDayList { get; set; }
        public List<Calendar> SiddhaJogaDayList { get; set; }
        public List<Calendar> MrityuJogaDayList { get; set; }
        public List<Calendar> AdhamJogaDayList { get; set; }
        public List<Calendar> YamaghataJogaDayList { get; set; }
        public List<Calendar> DagdhaJogaDayList { get; set; }
        public List<Calendar> UnfarobaleJogaDayList { get; set; }

        public List<Calendar> MuhurtaDayList { get; set; }

        public List<Calendar> Hora12Plus12DayList { get; set; }
        public List<Calendar> HoraEqualDayList { get; set; }
        public List<Calendar> HoraFrom6DayList { get; set; }
        public List<Calendar> Muhurta15Plus1530DayList { get; set; }
        public List<Calendar> MuhurtaEqual30DayList { get; set; }
        public List<Calendar> Muhurta30From6DayList { get; set; }
        public List<Calendar> Ghati60_30Plus30DayList { get; set; }
        public List<Calendar> Ghati60EqualDayList { get; set; }
        public List<Calendar> Ghati60From6DayList { get; set; }

        public List<MrityuBhagaData> MoonMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> SunMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> MercuryMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> VenusMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> MarsMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> JupiterMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> SaturnMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> RahuMeanMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> RahuTrueMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> KetuMeanMrityuBhagaDayList { get; set; }
        public List<MrityuBhagaData> KetuTrueMrityuBhagaDayList { get; set; }

        public Day(
            Profile sPerson,
            DateTime date,
            bool flag,
            DateTime? sunRise,
            DateTime? sunSet,
            List<PlanetCalendar> moonZodiakList,
            List<PlanetCalendar> moonZodiakRetroList,
            List<PlanetCalendar> moonNakshatraList,
            List<PlanetCalendar> moonPadaList,
            List<PlanetCalendar> sunZodiakList,
            List<PlanetCalendar> sunZodiakRetroList,
            List<PlanetCalendar> sunNakshatraList,
            List<PlanetCalendar> sunPadaList,
            List<PlanetCalendar> mercuryZodiakList,
            List<PlanetCalendar> mercuryZodiakRetroList,
            List<PlanetCalendar> mercuryNakshatraList,
            List<PlanetCalendar> mercuryPadaList,
            List<PlanetCalendar> venusZodiakList,
            List<PlanetCalendar> venusZodiakRetroList,
            List<PlanetCalendar> venusNakshatraList,
            List<PlanetCalendar> venusPadaList,
            List<PlanetCalendar> marsZodiakList,
            List<PlanetCalendar> marsZodiakRetroList,
            List<PlanetCalendar> marsNakshatraList,
            List<PlanetCalendar> marsPadaList,
            List<PlanetCalendar> jupiterZodiakList,
            List<PlanetCalendar> jupiterZodiakRetroList,
            List<PlanetCalendar> jupiterNakshatraList,
            List<PlanetCalendar> jupiterPadaList,
            List<PlanetCalendar> saturnZodiakList,
            List<PlanetCalendar> saturnZodiakRetroList,
            List<PlanetCalendar> saturnNakshatraList,
            List<PlanetCalendar> saturnPadaList,
            List<PlanetCalendar> rahuMeanZodiakList,
            List<PlanetCalendar> rahuMeanZodiakRetroList,
            List<PlanetCalendar> rahuMeanNakshatraList,
            List<PlanetCalendar> rahuMeanPadaList,
            List<PlanetCalendar> ketuMeanZodiakList,
            List<PlanetCalendar> ketuMeanZodiakRetroList,
            List<PlanetCalendar> ketuMeanNakshatraList,
            List<PlanetCalendar> ketuMeanPadaList,
            List<PlanetCalendar> rahuTrueZodiakList,
            List<PlanetCalendar> rahuTrueZodiakRetroList,
            List<PlanetCalendar> rahuTrueNakshatraList,
            List<PlanetCalendar> rahuTruePadaList,
            List<PlanetCalendar> ketuTrueZodiakList,
            List<PlanetCalendar> ketuTrueZodiakRetroList,
            List<PlanetCalendar> ketuTrueNakshatraList,
            List<PlanetCalendar> ketuTruePadaList,
            List<NakshatraCalendar> nakshatraList,
            List<TithiCalendar> tithiList,
            List<KaranaCalendar> karanaList,
            List<ChandraBalaCalendar> chandraBalaList,
            List<NityaYogaCalendar> njList,
            List<EclipseCalendar> eList,
            List<MasaCalendar> mList,
            List<ShunyaNakshatraCalendar> snList,
            List<ShunyaTithiCalendar> stList,
            List<MrityuBhagaData> moonMBDataList,
            List<MrityuBhagaData> sunMBDataList,
            List<MrityuBhagaData> mercuryMBDataList,
            List<MrityuBhagaData> venusMBDataList,
            List<MrityuBhagaData> marsMBDataList,
            List<MrityuBhagaData> jupiterMBDataList,
            List<MrityuBhagaData> saturnMBDataList,
            List<MrityuBhagaData> rahuMeanMBDataList,
            List<MrityuBhagaData> rahuTrueMBDataList,
            List<MrityuBhagaData> ketuMeanMBDataList,
            List<MrityuBhagaData> ketuTrueMBDataList
            )
        {
            Date = date;
            IsDayOfMonth = flag;
            SunRise = sunRise;
            SunSet = SunSet;

            double latitude, longitude;
            string timeZone = string.Empty;
            if (Utility.GetGeoCoordinateByLocationId(sPerson.PlaceOfLivingId, out latitude, out longitude))
            {
                timeZone = Utility.GetTimeZoneIdByGeoCoordinates(latitude, longitude);
                SunRise = Utility.CalculateSunriseForDateAndLocation(date, latitude, longitude, timeZone);
                SunSet = Utility.CalculateSunsetForDateAndLocation(date, latitude, longitude, timeZone);
            }

            List<PlanetCalendar> moonLagnaList = Utility.ClonePlanetCalendarList(moonZodiakList);
            List<PlanetCalendar> moonRetroLagnaList = Utility.ClonePlanetCalendarList(moonZodiakRetroList);

            List<PlanetCalendar> sunLagnaList = Utility.ClonePlanetCalendarList(sunZodiakList);
            List<PlanetCalendar> sunRetroLagnaList = Utility.ClonePlanetCalendarList(sunZodiakRetroList);

            List<PlanetCalendar> mercuryLagnaList = Utility.ClonePlanetCalendarList(mercuryZodiakList);
            List<PlanetCalendar> mercuryRetroLagnaList = Utility.ClonePlanetCalendarList(mercuryZodiakRetroList);

            List<PlanetCalendar> venusLagnaList = Utility.ClonePlanetCalendarList(venusZodiakList);
            List<PlanetCalendar> venusRetroLagnaList = Utility.ClonePlanetCalendarList(venusZodiakRetroList);

            List<PlanetCalendar> marsLagnaList = Utility.ClonePlanetCalendarList(marsZodiakList);
            List<PlanetCalendar> marsRetroLagnaList = Utility.ClonePlanetCalendarList(marsZodiakRetroList);

            List<PlanetCalendar> jupiterLagnaList = Utility.ClonePlanetCalendarList(jupiterZodiakList);
            List<PlanetCalendar> jupiterRetroLagnaList = Utility.ClonePlanetCalendarList(jupiterZodiakRetroList);

            List<PlanetCalendar> saturnLagnaList = Utility.ClonePlanetCalendarList(saturnZodiakList);
            List<PlanetCalendar> saturnRetroLagnaList = Utility.ClonePlanetCalendarList(saturnZodiakRetroList);

            List<PlanetCalendar> rahuMeanLagnaList = Utility.ClonePlanetCalendarList(rahuMeanZodiakList);
            List<PlanetCalendar> rahuMeanRetroLagnaList = Utility.ClonePlanetCalendarList(rahuMeanZodiakRetroList);

            List<PlanetCalendar> ketuMeanLagnaList = Utility.ClonePlanetCalendarList(ketuMeanZodiakList);
            List<PlanetCalendar> ketuMeanRetroLagnaList = Utility.ClonePlanetCalendarList(ketuMeanZodiakRetroList);

            List<PlanetCalendar> rahuTrueLagnaList = Utility.ClonePlanetCalendarList(rahuTrueZodiakList);
            List<PlanetCalendar> rahuTrueRetroLagnaList = Utility.ClonePlanetCalendarList(rahuTrueZodiakRetroList);

            List<PlanetCalendar> ketuTrueLagnaList = Utility.ClonePlanetCalendarList(ketuTrueZodiakList);
            List<PlanetCalendar> ketuTrueRetroLagnaList = Utility.ClonePlanetCalendarList(ketuTrueZodiakRetroList);

            List<PlanetCalendar> moonNTBList = Utility.ClonePlanetCalendarList(moonNakshatraList);
            MoonZodiakDayList = PreparePlanetZodiakDayList(moonZodiakList, date, false);
            MoonZodiakRetroDayList = PreparePlanetZodiakDayList(moonZodiakRetroList, date, false);
            MoonNakshatraDayList = PreparePlanetNakshatraDayList(moonNakshatraList, date);
            MoonPadaDayList = PreparePlanetPadaDayList(moonPadaList, date);
            MoonTaraBalaDayList = PreparePlanetTaraBalaDayList(moonNTBList, date);

            List<PlanetCalendar> sunNTBList = Utility.ClonePlanetCalendarList(sunNakshatraList);
            SunZodiakDayList = PreparePlanetZodiakDayList(sunZodiakList, date, false);
            SunZodiakRetroDayList = PreparePlanetZodiakDayList(sunZodiakRetroList, date, false);
            SunNakshatraDayList = PreparePlanetNakshatraDayList(sunNakshatraList, date);
            SunPadaDayList = PreparePlanetPadaDayList(sunPadaList, date);
            SunTaraBalaDayList = PreparePlanetTaraBalaDayList(sunNTBList, date);

            List<PlanetCalendar> mercuryNTBList = Utility.ClonePlanetCalendarList(mercuryNakshatraList);
            MercuryZodiakDayList = PreparePlanetZodiakDayList(mercuryZodiakList, date, false);
            MercuryZodiakRetroDayList = PreparePlanetZodiakDayList(mercuryZodiakRetroList, date, false);
            MercuryNakshatraDayList = PreparePlanetNakshatraDayList(mercuryNakshatraList, date);
            MercuryPadaDayList = PreparePlanetPadaDayList(mercuryPadaList, date);
            MercuryTaraBalaDayList = PreparePlanetTaraBalaDayList(mercuryNTBList, date);

            List<PlanetCalendar> venusNTBList = Utility.ClonePlanetCalendarList(venusNakshatraList);
            VenusZodiakDayList = PreparePlanetZodiakDayList(venusZodiakList, date, false);
            VenusZodiakRetroDayList = PreparePlanetZodiakDayList(venusZodiakRetroList, date, false);
            VenusNakshatraDayList = PreparePlanetNakshatraDayList(venusNakshatraList, date);
            VenusPadaDayList = PreparePlanetPadaDayList(venusPadaList, date);
            VenusTaraBalaDayList = PreparePlanetTaraBalaDayList(venusNTBList, date);

            List<PlanetCalendar> marsNTBList = Utility.ClonePlanetCalendarList(marsNakshatraList);
            MarsZodiakDayList = PreparePlanetZodiakDayList(marsZodiakList, date, false);
            MarsZodiakRetroDayList = PreparePlanetZodiakDayList(marsZodiakRetroList, date, false);
            MarsNakshatraDayList = PreparePlanetNakshatraDayList(marsNakshatraList, date);
            MarsPadaDayList = PreparePlanetPadaDayList(marsPadaList, date);
            MarsTaraBalaDayList = PreparePlanetTaraBalaDayList(marsNTBList, date);

            List<PlanetCalendar> jupiterNTBList = Utility.ClonePlanetCalendarList(jupiterNakshatraList);
            JupiterZodiakDayList = PreparePlanetZodiakDayList(jupiterZodiakList, date, false);
            JupiterZodiakRetroDayList = PreparePlanetZodiakDayList(jupiterZodiakRetroList, date, false);
            JupiterNakshatraDayList = PreparePlanetNakshatraDayList(jupiterNakshatraList, date);
            JupiterPadaDayList = PreparePlanetPadaDayList(jupiterPadaList, date);
            JupiterTaraBalaDayList = PreparePlanetTaraBalaDayList(jupiterNTBList, date);

            List<PlanetCalendar> saturnNTBList = Utility.ClonePlanetCalendarList(saturnNakshatraList);
            SaturnZodiakDayList = PreparePlanetZodiakDayList(saturnZodiakList, date, false);
            SaturnZodiakRetroDayList = PreparePlanetZodiakDayList(saturnZodiakRetroList, date, false);
            SaturnNakshatraDayList = PreparePlanetNakshatraDayList(saturnNakshatraList, date);
            SaturnPadaDayList = PreparePlanetPadaDayList(saturnPadaList, date);
            SaturnTaraBalaDayList = PreparePlanetTaraBalaDayList(saturnNTBList, date);

            List<PlanetCalendar> rahuMeanNTBList = Utility.ClonePlanetCalendarList(rahuMeanNakshatraList);
            RahuMeanZodiakDayList = PreparePlanetZodiakDayList(rahuMeanZodiakList, date, false);
            RahuMeanZodiakRetroDayList = PreparePlanetZodiakDayList(rahuMeanZodiakRetroList, date, false);
            RahuMeanNakshatraDayList = PreparePlanetNakshatraDayList(rahuMeanNakshatraList, date);
            RahuMeanPadaDayList = PreparePlanetPadaDayList(rahuMeanPadaList, date);
            RahuMeanTaraBalaDayList = PreparePlanetTaraBalaDayList(rahuMeanNTBList, date);

            List<PlanetCalendar> ketuMeanNTBList = Utility.ClonePlanetCalendarList(ketuMeanNakshatraList);
            KetuMeanZodiakDayList = PreparePlanetZodiakDayList(ketuMeanZodiakList, date, false);
            KetuMeanZodiakRetroDayList = PreparePlanetZodiakDayList(ketuMeanZodiakRetroList, date, false);
            KetuMeanNakshatraDayList = PreparePlanetNakshatraDayList(ketuMeanNakshatraList, date);
            KetuMeanPadaDayList = PreparePlanetPadaDayList(ketuMeanPadaList, date);
            KetuMeanTaraBalaDayList = PreparePlanetTaraBalaDayList(ketuMeanNTBList, date);

            List<PlanetCalendar> rahuTrueNTBList = Utility.ClonePlanetCalendarList(rahuTrueNakshatraList);
            RahuTrueZodiakDayList = PreparePlanetZodiakDayList(rahuTrueZodiakList, date, false);
            RahuTrueZodiakRetroDayList = PreparePlanetZodiakDayList(rahuTrueZodiakRetroList, date, false);
            RahuTrueNakshatraDayList = PreparePlanetNakshatraDayList(rahuTrueNakshatraList, date);
            RahuTruePadaDayList = PreparePlanetPadaDayList(rahuTruePadaList, date);
            RahuTrueTaraBalaDayList = PreparePlanetTaraBalaDayList(rahuTrueNTBList, date);

            List<PlanetCalendar> ketuTrueNTBList = Utility.ClonePlanetCalendarList(ketuTrueNakshatraList);
            KetuTrueZodiakDayList = PreparePlanetZodiakDayList(ketuTrueZodiakList, date, false);
            KetuTrueZodiakRetroDayList = PreparePlanetZodiakDayList(ketuTrueZodiakRetroList, date, false);
            KetuTrueNakshatraDayList = PreparePlanetNakshatraDayList(ketuTrueNakshatraList, date);
            KetuTruePadaDayList = PreparePlanetPadaDayList(ketuTruePadaList, date);
            KetuTrueTaraBalaDayList = PreparePlanetTaraBalaDayList(ketuTrueNTBList, date);

            MoonZodiakLagnaDayList = PreparePlanetZodiakDayList(moonLagnaList, date, true);
            MoonZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(moonRetroLagnaList, date, true);

            SunZodiakLagnaDayList = PreparePlanetZodiakDayList(sunLagnaList, date, true);
            SunZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(sunRetroLagnaList, date, true);

            MercuryZodiakLagnaDayList = PreparePlanetZodiakDayList(mercuryLagnaList, date, true);
            MercuryZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(mercuryRetroLagnaList, date, true);

            VenusZodiakLagnaDayList = PreparePlanetZodiakDayList(venusLagnaList, date, true);
            VenusZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(venusRetroLagnaList, date, true);

            MarsZodiakLagnaDayList = PreparePlanetZodiakDayList(marsLagnaList, date, true);
            MarsZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(marsRetroLagnaList, date, true);

            JupiterZodiakLagnaDayList = PreparePlanetZodiakDayList(jupiterLagnaList, date, true);
            JupiterZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(jupiterRetroLagnaList, date, true);

            SaturnZodiakLagnaDayList = PreparePlanetZodiakDayList(saturnLagnaList, date, true);
            SaturnZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(saturnRetroLagnaList, date, true);

            RahuMeanZodiakLagnaDayList = PreparePlanetZodiakDayList(rahuMeanLagnaList, date, true);
            RahuMeanZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(rahuMeanRetroLagnaList, date, true);

            KetuMeanZodiakLagnaDayList = PreparePlanetZodiakDayList(ketuMeanLagnaList, date, true);
            KetuMeanZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(ketuMeanRetroLagnaList, date, true);

            RahuTrueZodiakLagnaDayList = PreparePlanetZodiakDayList(rahuTrueLagnaList, date, true);
            RahuTrueZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(rahuTrueRetroLagnaList, date, true);

            KetuTrueZodiakLagnaDayList = PreparePlanetZodiakDayList(ketuTrueLagnaList, date, true);
            KetuTrueZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(ketuTrueRetroLagnaList, date, true);

            NakshatraDayList = PrepareNakshatraDayList(nakshatraList, date);
            TaraBalaDayList = PrepareTaraBalaDayList(nakshatraList, date);
            TithiDayList = PrepareTithiDayList(tithiList, date);
            KaranaDayList = PrepareKaranaDayList(karanaList, date);
            ChandraBalaDayList = PrepareChandraBalaDayList(chandraBalaList, date);
            NityaJogaDayList = PrepareNityaYogaDayList(njList, date);
            EclipseDayList = PrepareEclipseDayList(eList, date);
            MasaDayList = PrepareMasaDayList(mList, date);
            ShunyaNakshatraDayList = PrepareShunyaNakshatraDayList(snList, date);
            ShunyaTithiDayList = PrepareShunyaTithiDayList(stList, date);

            MoonMrityuBhagaDayList = PrepareMrityuBhagaDayList(moonMBDataList, date);
            SunMrityuBhagaDayList = PrepareMrityuBhagaDayList(sunMBDataList, date);
            MercuryMrityuBhagaDayList = PrepareMrityuBhagaDayList(mercuryMBDataList, date);
            VenusMrityuBhagaDayList = PrepareMrityuBhagaDayList(venusMBDataList, date);
            MarsMrityuBhagaDayList = PrepareMrityuBhagaDayList(marsMBDataList, date);
            JupiterMrityuBhagaDayList = PrepareMrityuBhagaDayList(jupiterMBDataList, date);
            SaturnMrityuBhagaDayList = PrepareMrityuBhagaDayList(saturnMBDataList, date);
            RahuMeanMrityuBhagaDayList = PrepareMrityuBhagaDayList(rahuMeanMBDataList, date);
            RahuTrueMrityuBhagaDayList = PrepareMrityuBhagaDayList(rahuTrueMBDataList, date);
            KetuMeanMrityuBhagaDayList = PrepareMrityuBhagaDayList(ketuMeanMBDataList, date);
            KetuTrueMrityuBhagaDayList = PrepareMrityuBhagaDayList(ketuTrueMBDataList, date);
        }

        // for year's tranzits
        public Day(
            DateTime date,
            List<PlanetCalendar> moonZodiakList,
            List<PlanetCalendar> moonZodiakRetroList,
            List<PlanetCalendar> moonNakshatraList,
            List<PlanetCalendar> moonPadaList,
            List<PlanetCalendar> sunZodiakList,
            List<PlanetCalendar> sunZodiakRetroList,
            List<PlanetCalendar> sunNakshatraList,
            List<PlanetCalendar> sunPadaList,
            List<PlanetCalendar> mercuryZodiakList,
            List<PlanetCalendar> mercuryZodiakRetroList,
            List<PlanetCalendar> mercuryNakshatraList,
            List<PlanetCalendar> mercuryPadaList,
            List<PlanetCalendar> venusZodiakList,
            List<PlanetCalendar> venusZodiakRetroList,
            List<PlanetCalendar> venusNakshatraList,
            List<PlanetCalendar> venusPadaList,
            List<PlanetCalendar> marsZodiakList,
            List<PlanetCalendar> marsZodiakRetroList,
            List<PlanetCalendar> marsNakshatraList,
            List<PlanetCalendar> marsPadaList,
            List<PlanetCalendar> jupiterZodiakList,
            List<PlanetCalendar> jupiterZodiakRetroList,
            List<PlanetCalendar> jupiterNakshatraList,
            List<PlanetCalendar> jupiterPadaList,
            List<PlanetCalendar> saturnZodiakList,
            List<PlanetCalendar> saturnZodiakRetroList,
            List<PlanetCalendar> saturnNakshatraList,
            List<PlanetCalendar> saturnPadaList,
            List<PlanetCalendar> rahuMeanZodiakList,
            List<PlanetCalendar> rahuMeanZodiakRetroList,
            List<PlanetCalendar> rahuMeanNakshatraList,
            List<PlanetCalendar> rahuMeanPadaList,
            List<PlanetCalendar> ketuMeanZodiakList,
            List<PlanetCalendar> ketuMeanZodiakRetroList,
            List<PlanetCalendar> ketuMeanNakshatraList,
            List<PlanetCalendar> ketuMeanPadaList,
            List<PlanetCalendar> rahuTrueZodiakList,
            List<PlanetCalendar> rahuTrueZodiakRetroList,
            List<PlanetCalendar> rahuTrueNakshatraList,
            List<PlanetCalendar> rahuTruePadaList,
            List<PlanetCalendar> ketuTrueZodiakList,
            List<PlanetCalendar> ketuTrueZodiakRetroList,
            List<PlanetCalendar> ketuTrueNakshatraList,
            List<PlanetCalendar> ketuTruePadaList,
            List<MasaCalendar> masaList,
            List<ShunyaNakshatraCalendar> shuNakList,
            List<ShunyaTithiCalendar> shuTiList,
            List<MrityuBhagaData> moonMBDataList,
            List<MrityuBhagaData> sunMBDataList,
            List<MrityuBhagaData> mercuryMBDataList,
            List<MrityuBhagaData> venusMBDataList,
            List<MrityuBhagaData> marsMBDataList,
            List<MrityuBhagaData> jupiterMBDataList,
            List<MrityuBhagaData> saturnMBDataList,
            List<MrityuBhagaData> rahuMeanMBDataList,
            List<MrityuBhagaData> rahuTrueMBDataList,
            List<MrityuBhagaData> ketuMeanMBDataList,
            List<MrityuBhagaData> ketuTrueMBDataList
            )
        {
            Date = date;

            List<PlanetCalendar> moonLagnaList = Utility.ClonePlanetCalendarList(moonZodiakList);
            List<PlanetCalendar> moonRetroLagnaList = Utility.ClonePlanetCalendarList(moonZodiakRetroList);

            List<PlanetCalendar> sunLagnaList = Utility.ClonePlanetCalendarList(sunZodiakList);
            List<PlanetCalendar> sunRetroLagnaList = Utility.ClonePlanetCalendarList(sunZodiakRetroList);

            List<PlanetCalendar> mercuryLagnaList = Utility.ClonePlanetCalendarList(mercuryZodiakList);
            List<PlanetCalendar> mercuryRetroLagnaList = Utility.ClonePlanetCalendarList(mercuryZodiakRetroList);

            List<PlanetCalendar> venusLagnaList = Utility.ClonePlanetCalendarList(venusZodiakList);
            List<PlanetCalendar> venusRetroLagnaList = Utility.ClonePlanetCalendarList(venusZodiakRetroList);

            List<PlanetCalendar> marsLagnaList = Utility.ClonePlanetCalendarList(marsZodiakList);
            List<PlanetCalendar> marsRetroLagnaList = Utility.ClonePlanetCalendarList(marsZodiakRetroList);

            List<PlanetCalendar> jupiterLagnaList = Utility.ClonePlanetCalendarList(jupiterZodiakList);
            List<PlanetCalendar> jupiterRetroLagnaList = Utility.ClonePlanetCalendarList(jupiterZodiakRetroList);

            List<PlanetCalendar> saturnLagnaList = Utility.ClonePlanetCalendarList(saturnZodiakList);
            List<PlanetCalendar> saturnRetroLagnaList = Utility.ClonePlanetCalendarList(saturnZodiakRetroList);

            List<PlanetCalendar> rahuMeanLagnaList = Utility.ClonePlanetCalendarList(rahuMeanZodiakList);
            List<PlanetCalendar> rahuMeanRetroLagnaList = Utility.ClonePlanetCalendarList(rahuMeanZodiakRetroList);

            List<PlanetCalendar> ketuMeanLagnaList = Utility.ClonePlanetCalendarList(ketuMeanZodiakList);
            List<PlanetCalendar> ketuMeanRetroLagnaList = Utility.ClonePlanetCalendarList(ketuMeanZodiakRetroList);

            List<PlanetCalendar> rahuTrueLagnaList = Utility.ClonePlanetCalendarList(rahuTrueZodiakList);
            List<PlanetCalendar> rahuTrueRetroLagnaList = Utility.ClonePlanetCalendarList(rahuTrueZodiakRetroList);

            List<PlanetCalendar> ketuTrueLagnaList = Utility.ClonePlanetCalendarList(ketuTrueZodiakList);
            List<PlanetCalendar> ketuTrueRetroLagnaList = Utility.ClonePlanetCalendarList(ketuTrueZodiakRetroList);

            List<PlanetCalendar> moonNTBList = Utility.ClonePlanetCalendarList(moonNakshatraList);
            MoonZodiakDayList = PreparePlanetZodiakDayList(moonZodiakList, date, false);
            MoonZodiakRetroDayList = PreparePlanetZodiakDayList(moonZodiakRetroList, date, false);
            MoonNakshatraDayList = PreparePlanetNakshatraDayList(moonNakshatraList, date);
            MoonPadaDayList = PreparePlanetPadaDayList(moonPadaList, date);
            MoonTaraBalaDayList = PreparePlanetTaraBalaDayList(moonNTBList, date);

            List<PlanetCalendar> sunNTBList = Utility.ClonePlanetCalendarList(sunNakshatraList);
            SunZodiakDayList = PreparePlanetZodiakDayList(sunZodiakList, date, false);
            SunZodiakRetroDayList = PreparePlanetZodiakDayList(sunZodiakRetroList, date, false);
            SunNakshatraDayList = PreparePlanetNakshatraDayList(sunNakshatraList, date);
            SunPadaDayList = PreparePlanetPadaDayList(sunPadaList, date);
            SunTaraBalaDayList = PreparePlanetTaraBalaDayList(sunNTBList, date);

            List<PlanetCalendar> mercuryNTBList = Utility.ClonePlanetCalendarList(mercuryNakshatraList);
            MercuryZodiakDayList = PreparePlanetZodiakDayList(mercuryZodiakList, date, false);
            MercuryZodiakRetroDayList = PreparePlanetZodiakDayList(mercuryZodiakRetroList, date, false);
            MercuryNakshatraDayList = PreparePlanetNakshatraDayList(mercuryNakshatraList, date);
            MercuryPadaDayList = PreparePlanetPadaDayList(mercuryPadaList, date);
            MercuryTaraBalaDayList = PreparePlanetTaraBalaDayList(mercuryNTBList, date);

            List<PlanetCalendar> venusNTBList = Utility.ClonePlanetCalendarList(venusNakshatraList);
            VenusZodiakDayList = PreparePlanetZodiakDayList(venusZodiakList, date, false);
            VenusZodiakRetroDayList = PreparePlanetZodiakDayList(venusZodiakRetroList, date, false);
            VenusNakshatraDayList = PreparePlanetNakshatraDayList(venusNakshatraList, date);
            VenusPadaDayList = PreparePlanetPadaDayList(venusPadaList, date);
            VenusTaraBalaDayList = PreparePlanetTaraBalaDayList(venusNTBList, date);

            List<PlanetCalendar> marsNTBList = Utility.ClonePlanetCalendarList(marsNakshatraList);
            MarsZodiakDayList = PreparePlanetZodiakDayList(marsZodiakList, date, false);
            MarsZodiakRetroDayList = PreparePlanetZodiakDayList(marsZodiakRetroList, date, false);
            MarsNakshatraDayList = PreparePlanetNakshatraDayList(marsNakshatraList, date);
            MarsPadaDayList = PreparePlanetPadaDayList(marsPadaList, date);
            MarsTaraBalaDayList = PreparePlanetTaraBalaDayList(marsNTBList, date);

            List<PlanetCalendar> jupiterNTBList = Utility.ClonePlanetCalendarList(jupiterNakshatraList);
            JupiterZodiakDayList = PreparePlanetZodiakDayList(jupiterZodiakList, date, false);
            JupiterZodiakRetroDayList = PreparePlanetZodiakDayList(jupiterZodiakRetroList, date, false);
            JupiterNakshatraDayList = PreparePlanetNakshatraDayList(jupiterNakshatraList, date);
            JupiterPadaDayList = PreparePlanetPadaDayList(jupiterPadaList, date);
            JupiterTaraBalaDayList = PreparePlanetTaraBalaDayList(jupiterNTBList, date);

            List<PlanetCalendar> saturnNTBList = Utility.ClonePlanetCalendarList(saturnNakshatraList);
            SaturnZodiakDayList = PreparePlanetZodiakDayList(saturnZodiakList, date, false);
            SaturnZodiakRetroDayList = PreparePlanetZodiakDayList(saturnZodiakRetroList, date, false);
            SaturnNakshatraDayList = PreparePlanetNakshatraDayList(saturnNakshatraList, date);
            SaturnPadaDayList = PreparePlanetPadaDayList(saturnPadaList, date);
            SaturnTaraBalaDayList = PreparePlanetTaraBalaDayList(saturnNTBList, date);

            List<PlanetCalendar> rahuMeanNTBList = Utility.ClonePlanetCalendarList(rahuMeanNakshatraList);
            RahuMeanZodiakDayList = PreparePlanetZodiakDayList(rahuMeanZodiakList, date, false);
            RahuMeanZodiakRetroDayList = PreparePlanetZodiakDayList(rahuMeanZodiakRetroList, date, false);
            RahuMeanNakshatraDayList = PreparePlanetNakshatraDayList(rahuMeanNakshatraList, date);
            RahuMeanPadaDayList = PreparePlanetPadaDayList(rahuMeanPadaList, date);
            RahuMeanTaraBalaDayList = PreparePlanetTaraBalaDayList(rahuMeanNTBList, date);

            List<PlanetCalendar> ketuMeanNTBList = Utility.ClonePlanetCalendarList(ketuMeanNakshatraList);
            KetuMeanZodiakDayList = PreparePlanetZodiakDayList(ketuMeanZodiakList, date, false);
            KetuMeanZodiakRetroDayList = PreparePlanetZodiakDayList(ketuMeanZodiakRetroList, date, false);
            KetuMeanNakshatraDayList = PreparePlanetNakshatraDayList(ketuMeanNakshatraList, date);
            KetuMeanPadaDayList = PreparePlanetPadaDayList(ketuMeanPadaList, date);
            KetuMeanTaraBalaDayList = PreparePlanetTaraBalaDayList(ketuMeanNTBList, date);

            List<PlanetCalendar> rahuTrueNTBList = Utility.ClonePlanetCalendarList(rahuTrueNakshatraList);
            RahuTrueZodiakDayList = PreparePlanetZodiakDayList(rahuTrueZodiakList, date, false);
            RahuTrueZodiakRetroDayList = PreparePlanetZodiakDayList(rahuTrueZodiakRetroList, date, false);
            RahuTrueNakshatraDayList = PreparePlanetNakshatraDayList(rahuTrueNakshatraList, date);
            RahuTruePadaDayList = PreparePlanetPadaDayList(rahuTruePadaList, date);
            RahuTrueTaraBalaDayList = PreparePlanetTaraBalaDayList(rahuTrueNTBList, date);

            List<PlanetCalendar> ketuTrueNTBList = Utility.ClonePlanetCalendarList(ketuTrueNakshatraList);
            KetuTrueZodiakDayList = PreparePlanetZodiakDayList(ketuTrueZodiakList, date, false);
            KetuTrueZodiakRetroDayList = PreparePlanetZodiakDayList(ketuTrueZodiakRetroList, date, false);
            KetuTrueNakshatraDayList = PreparePlanetNakshatraDayList(ketuTrueNakshatraList, date);
            KetuTruePadaDayList = PreparePlanetPadaDayList(ketuTruePadaList, date);
            KetuTrueTaraBalaDayList = PreparePlanetTaraBalaDayList(ketuTrueNTBList, date);

            MoonZodiakLagnaDayList = PreparePlanetZodiakDayList(moonLagnaList, date, true);
            MoonZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(moonRetroLagnaList, date, true);

            SunZodiakLagnaDayList = PreparePlanetZodiakDayList(sunLagnaList, date, true);
            SunZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(sunRetroLagnaList, date, true);

            MercuryZodiakLagnaDayList = PreparePlanetZodiakDayList(mercuryLagnaList, date, true);
            MercuryZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(mercuryRetroLagnaList, date, true);

            VenusZodiakLagnaDayList = PreparePlanetZodiakDayList(venusLagnaList, date, true);
            VenusZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(venusRetroLagnaList, date, true);

            MarsZodiakLagnaDayList = PreparePlanetZodiakDayList(marsLagnaList, date, true);
            MarsZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(marsRetroLagnaList, date, true);

            JupiterZodiakLagnaDayList = PreparePlanetZodiakDayList(jupiterLagnaList, date, true);
            JupiterZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(jupiterRetroLagnaList, date, true);

            SaturnZodiakLagnaDayList = PreparePlanetZodiakDayList(saturnLagnaList, date, true);
            SaturnZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(saturnRetroLagnaList, date, true);

            RahuMeanZodiakLagnaDayList = PreparePlanetZodiakDayList(rahuMeanLagnaList, date, true);
            RahuMeanZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(rahuMeanRetroLagnaList, date, true);

            KetuMeanZodiakLagnaDayList = PreparePlanetZodiakDayList(ketuMeanLagnaList, date, true);
            KetuMeanZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(ketuMeanRetroLagnaList, date, true);

            RahuTrueZodiakLagnaDayList = PreparePlanetZodiakDayList(rahuTrueLagnaList, date, true);
            RahuTrueZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(rahuTrueRetroLagnaList, date, true);

            KetuTrueZodiakLagnaDayList = PreparePlanetZodiakDayList(ketuTrueLagnaList, date, true);
            KetuTrueZodiakRetroLagnaDayList = PreparePlanetZodiakDayList(ketuTrueRetroLagnaList, date, true);

            MasaDayList = PrepareMasaDayList(masaList, date);
            ShunyaNakshatraDayList = PrepareShunyaNakshatraDayList(shuNakList, date);
            ShunyaTithiDayList = PrepareShunyaTithiDayList(shuTiList, date);

            MoonMrityuBhagaDayList = PrepareMrityuBhagaDayList(moonMBDataList, date);
            SunMrityuBhagaDayList = PrepareMrityuBhagaDayList(sunMBDataList, date);
            MercuryMrityuBhagaDayList = PrepareMrityuBhagaDayList(mercuryMBDataList, date);
            VenusMrityuBhagaDayList = PrepareMrityuBhagaDayList(venusMBDataList, date);
            MarsMrityuBhagaDayList = PrepareMrityuBhagaDayList(marsMBDataList, date);
            JupiterMrityuBhagaDayList = PrepareMrityuBhagaDayList(jupiterMBDataList, date);
            SaturnMrityuBhagaDayList = PrepareMrityuBhagaDayList(saturnMBDataList, date);
            RahuMeanMrityuBhagaDayList = PrepareMrityuBhagaDayList(rahuMeanMBDataList, date);
            RahuTrueMrityuBhagaDayList = PrepareMrityuBhagaDayList(rahuTrueMBDataList, date);
            KetuMeanMrityuBhagaDayList = PrepareMrityuBhagaDayList(ketuMeanMBDataList, date);
            KetuTrueMrityuBhagaDayList = PrepareMrityuBhagaDayList(ketuTrueMBDataList, date);
        }

        private List<MrityuBhagaData> PrepareMrityuBhagaDayList(List<MrityuBhagaData> mbList, DateTime date)
        {
            return mbList.Where(i => i.DateTo >= date && i.DateFrom <= date.AddDays(+1)).ToList(); 
        }

        private List<Calendar> PreparePlanetZodiakDayList(List<PlanetCalendar> pcList, DateTime date, bool isLagna)
        {
            List<Calendar> resList = new List<Calendar>();
            List<PlanetCalendar> pList =  pcList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            foreach (PlanetCalendar pc in pList)
            {
                if (isLagna)
                {
                    List<Zodiak> swappedZodiakLagnaList = Utility.SwappingZodiakList(CacheLoad._zodiakList.ToList(), CacheLoad._birthLagnaId);
                    pc.LagnaDom = GetPlanetDom(swappedZodiakLagnaList, pc.ZodiakCode);
                    pc.ColorCode = (EColor)GetPlanetColor(pc.PlanetCode, pc.LagnaDom);
                }
                else
                {
                    List<Zodiak> swappedZodiakList = Utility.SwappingZodiakList(CacheLoad._zodiakList.ToList(), CacheLoad._birthZodiakMoonId);
                    pc.Dom = GetPlanetDom(swappedZodiakList, pc.ZodiakCode);
                    pc.ColorCode = (EColor)GetPlanetColor(pc.PlanetCode, pc.Dom);
                }
                resList.Add(pc);
            }
            return resList;
        }

        private List<Calendar> PreparePlanetNakshatraDayList(List<PlanetCalendar> pcList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<PlanetCalendar> pList = pcList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            foreach (PlanetCalendar pc in pList)
            {
                pc.ColorCode = (EColor)(CacheLoad._nakshatraList.Where(i => i.Id == (int)pc.NakshatraCode).FirstOrDefault()?.ColorId ?? 0);
                resList.Add(pc);
            }
            return resList;
        }

        private List<Calendar> PreparePlanetPadaDayList(List<PlanetCalendar> pcList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<PlanetCalendar> pList = pcList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            foreach (PlanetCalendar pc in pList)
            {
                pc.ColorCode = GetPadaColor(pc.PadaId);
                resList.Add(pc);
            }
            return resList;
        }

        private List<Calendar> PreparePlanetTaraBalaDayList(List<PlanetCalendar> pcList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<PlanetCalendar> pList = pcList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            List<Nakshatra> swappedNakshatraList = SwappingNakshatraList(CacheLoad._nakshatraList.ToList(), CacheLoad._birthNakshatraMoonId);
            int[,] taraBala2DArray = Make2DArrayFromNakshatraList(swappedNakshatraList);
            foreach (PlanetCalendar pc in pList)
            {
                int tbId = GetTaraBalaNumber(taraBala2DArray, (int)pc.NakshatraCode);
                pc.TaraBalaId = (tbId + 1);
                pc.ColorCode = (EColor)(CacheLoad._taraBalaList.Where(i => i.Id == (tbId + 1)).FirstOrDefault()?.ColorId ?? 0);
                pc.TaraBalaPercent = GetTaraBalaPercentAmount(taraBala2DArray, (int)pc.NakshatraCode);
                if (pc.TaraBalaPercent == 25 && pc.ColorCode == EColor.RED)
                {
                    pc.ColorCode = EColor.PINK;
                }
                resList.Add(pc);
            }
            return resList;
        }

        private EColor GetPadaColor(int padaId)
        {
            return (EColor)(CacheLoad._padaList.Where(i => i.Id == padaId).FirstOrDefault()?.ColorId ?? 0);
        }

        private int GetPlanetDom(List<Zodiak> swappedZodiakList, EZodiak zCode)
        {
            return (swappedZodiakList.FindIndex(i => i.Code.Equals(zCode.ToString())) + 1);
        }

        private int GetPlanetColor(EPlanet pCode, int pDom)
        {
            if (pCode == EPlanet.RAHUTRUE)
                pCode = EPlanet.RAHUMEAN;
            if (pCode == EPlanet.KETUTRUE)
                pCode = EPlanet.KETUMEAN;
            return CacheLoad._tranzitList.Where(i => i.PlanetId == (int)pCode && i.Dom == pDom).FirstOrDefault()?.ColorId ?? 0;
        }

        private List<Calendar> PrepareNakshatraDayList(List<NakshatraCalendar> nList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<NakshatraCalendar> nakList = nList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            foreach (NakshatraCalendar nak in nakList)
            {
                resList.Add(nak);
            }
            return resList;
        }

        private List<Calendar> PrepareTaraBalaDayList(List<NakshatraCalendar> nList, DateTime date)
        {
            List<Calendar> dayList = new List<Calendar>();
            List<NakshatraCalendar> ndList = nList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            List<Nakshatra> swappedNakshatraList = SwappingNakshatraList(CacheLoad._nakshatraList.ToList(), CacheLoad._birthNakshatraMoonId);
            int[,] taraBala2DArray = Make2DArrayFromNakshatraList(swappedNakshatraList);
            foreach (NakshatraCalendar nc in ndList)
            {
                TaraBalaCalendar tempDay = CopyNakshatraElementToTaraBala(nc);
                int tbId = GetTaraBalaNumber(taraBala2DArray, (int)nc.NakshatraCode);
                //if (tempDay.TaraBalaId == 0 && tempDay.Percent == 0)
                {
                    tempDay.TaraBalaId = tbId + 1;
                    tempDay.ColorCode = (EColor)(CacheLoad._taraBalaList.Where(i => i.Id == (tbId + 1)).FirstOrDefault()?.ColorId ?? 0);
                    tempDay.Percent = GetTaraBalaPercentAmount(taraBala2DArray, (int)nc.NakshatraCode);

                    if (tempDay.Percent == 25 && tempDay.ColorCode == EColor.RED)
                        tempDay.ColorCode = EColor.PINK;

                }
                dayList.Add(tempDay);
            }
            return dayList;
        }

        private List<Nakshatra> SwappingNakshatraList(List<Nakshatra> nList, int id)
        {
            List<Nakshatra> newList = new List<Nakshatra>();
            newList.AddRange(nList.Where(s => s.Id >= id).ToList());
            newList.AddRange(nList.Where(s => s.Id < id).ToList());
            return newList;
        }

        private int[,] Make2DArrayFromNakshatraList(List<Nakshatra> nList)
        {
            int i = 0;
            int[,] tempArray = new int[9, 3];
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 9; row++)
                {
                    tempArray[row, col] = nList[i].Id;
                    i++;
                }
            }
            return tempArray;
        }

        private int GetTaraBalaNumber(int[,] n2DArray, int nNum)
        {
            int rowIndex = -1, colIndex = -1;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (n2DArray[row, col] == nNum)
                    {
                        rowIndex = row;
                        colIndex = col;
                        break;
                    }
                }
            }
            return rowIndex;
        }

        private int GetTaraBalaPercentAmount(int[,] n2DArray, int nNum)
        {
            int colIndex = -1;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (n2DArray[row, col] == nNum)
                    {
                        colIndex = col;
                        break;
                    }
                }
            }
            if (colIndex == 0)
                return 100;
            else if (colIndex == 1)
                return 50;
            else
                return 25;
        }

        private TaraBalaCalendar CopyNakshatraElementToTaraBala(NakshatraCalendar el)
        {
            return new TaraBalaCalendar
            {
                DateStart = el.DateStart,
                DateEnd = el.DateEnd,
                ColorCode = EColor.NOCOLOR,
                NakshatraCode = el.NakshatraCode,
                TaraBalaId = 0,
                Percent = 0
            };
        }

        private List<Calendar> PrepareTithiDayList(List<TithiCalendar> tcList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<TithiCalendar> tList = tcList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            foreach (TithiCalendar ti in tList)
            {
                resList.Add(ti);
            }
            return resList;
        }

        private List<Calendar> PrepareKaranaDayList(List<KaranaCalendar> kList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<KaranaCalendar> kaList = kList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            foreach (KaranaCalendar ka in kaList)
            {
                resList.Add(ka);
            }
            return resList;
        }

        private List<Calendar> PrepareChandraBalaDayList(List<ChandraBalaCalendar> cbList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<ChandraBalaCalendar> listForDay = cbList.Where(s => s.DateEnd >= date && s.DateStart <= date.AddDays(+1)).ToList();
            List<Zodiak> swappedZodiakList = Utility.SwappingZodiakList(CacheLoad._zodiakList.ToList(), CacheLoad._birthZodiakMoonId);
            foreach (ChandraBalaCalendar cbc in listForDay)
            {
                cbc.ColorCode = GetChandraBalaColor(swappedZodiakList, cbc.ZodiakCode);
                cbc.DomNumber = swappedZodiakList.FindIndex(i => i.Id == (int)cbc.ZodiakCode) + 1;
                cbc.Dom = GetChandraBalaDom(swappedZodiakList, cbc.ZodiakCode);
            }
            foreach (ChandraBalaCalendar cbc in listForDay)
            {
                resList.Add(cbc);
            }
            return resList;
        }

        private EColor GetChandraBalaColor(List<Zodiak> swappedZodiakList, EZodiak zCode)
        {
            int index = swappedZodiakList.FindIndex(i => i.Id == (int)zCode);
            if (index == 5 || index == 7 || index == 11 || swappedZodiakList[index].Code.Equals(EZodiak.SCO.ToString()))
            {
                return EColor.RED;
            }
            else
            {
                return EColor.GREEN;
            }
        }

        private string GetChandraBalaDom(List<Zodiak> swappedZodiakList, EZodiak zCode)
        {
            string dom = string.Empty;
            int index = swappedZodiakList.FindIndex(i => i.Id == (int)zCode);
            dom = String.Format(Utility.GetLocalizedText("Moon in {0} house", (ELanguage)Utility.GetActiveLanguageCode(CacheLoad._appSettingList)), (index + 1));
            if (index >= 0 && swappedZodiakList[index].Code.Equals(EZodiak.SCO.ToString()))
            {
                dom = dom + ", Sco";
            }
            return dom;
        }

        private List<Calendar> PrepareNityaYogaDayList(List<NityaYogaCalendar> njcList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<NityaYogaCalendar> niList = njcList.Where(i => i.DateEnd >= date && i.DateStart <= date.AddDays(+1)).ToList();
            foreach (NityaYogaCalendar nc in niList)
            {
                resList.Add(nc);
            }
            return resList;
        }

        private List<Calendar> PrepareEclipseDayList(List<EclipseCalendar> ecList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            List<EclipseCalendar> eList = ecList.Where(i => i.DateStart >= date && i.DateStart <= date.AddDays(+1)).ToList();
            foreach (EclipseCalendar ec in eList)
            {
                resList.Add(ec);
            }
            return resList;
        }

        private List<Calendar> PrepareMasaDayList(List<MasaCalendar> mList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            TimeSpan dayLong = new TimeSpan(23, 59, 59);
            List<MasaCalendar> maList = mList.Where(i => date.Between(i.DateStart, i.DateEnd) || date.Add(dayLong).Between(i.DateStart, i.DateEnd)).ToList();
            List<MasaCalendar> maClonedList = Utility.CloneMasaCalendarList(maList);
            foreach (MasaCalendar ma in maClonedList)
            {
                ma.ColorCode = Utility.GetMasaColorById(ma.MasaId);
                resList.Add(ma);
            }
            return resList;
        }

        private List<Calendar> PrepareShunyaNakshatraDayList(List<ShunyaNakshatraCalendar> snList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            TimeSpan dayLong = new TimeSpan(23, 59, 59);
            TimeSpan dayMiddle = new TimeSpan(12, 0, 0);
            List<ShunyaNakshatraCalendar> dsnList = snList.Where(i => date.Between(i.DateStart, i.DateEnd) || date.Add(dayMiddle).Between(i.DateStart, i.DateEnd) || date.Add(dayLong).Between(i.DateStart, i.DateEnd)).ToList();
            foreach (ShunyaNakshatraCalendar sn in dsnList)
            {
                resList.Add(sn);
            }
            return resList;
        }

        private List<Calendar> PrepareShunyaTithiDayList(List<ShunyaTithiCalendar> stList, DateTime date)
        {
            List<Calendar> resList = new List<Calendar>();
            TimeSpan dayLong = new TimeSpan(23, 59, 59);
            TimeSpan dayMiddle = new TimeSpan(12, 0, 0);
            List<ShunyaTithiCalendar> dstList = stList.Where(i => date.Between(i.DateStart, i.DateEnd) || date.Add(dayMiddle).Between(i.DateStart, i.DateEnd) || date.Add(dayLong).Between(i.DateStart, i.DateEnd)).ToList();
            foreach (ShunyaTithiCalendar st in dstList)
            {
                resList.Add(st);
            }
            return resList;
        }

    }
}
