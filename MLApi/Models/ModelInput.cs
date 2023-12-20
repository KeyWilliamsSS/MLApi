using Microsoft.ML.Data;

namespace MLApi.Models
{
    /// <summary>
    /// model input class for HousePrice.
    /// </summary>

    #region model input class

    public class ModelInput
    {
        [LoadColumn(0)]
        [ColumnName(@"longitude")]
        public float Longitude { get; set; }

        [LoadColumn(1)]
        [ColumnName(@"latitude")]
        public float Latitude { get; set; }

        [LoadColumn(2)]
        [ColumnName(@"housing_median_age")]
        public float Housing_median_age { get; set; }

        [LoadColumn(3)]
        [ColumnName(@"total_rooms")]
        public float Total_rooms { get; set; }

        [LoadColumn(4)]
        [ColumnName(@"total_bedrooms")]
        public float Total_bedrooms { get; set; }

        [LoadColumn(5)]
        [ColumnName(@"population")]
        public float Population { get; set; }

        [LoadColumn(6)]
        [ColumnName(@"households")]
        public float Households { get; set; }

        [LoadColumn(7)]
        [ColumnName(@"median_income")]
        public float Median_income { get; set; }

        [LoadColumn(8)]
        [ColumnName(@"median_house_value")]
        public float Median_house_value { get; set; }

        [LoadColumn(9)]
        [ColumnName(@"ocean_proximity")]
        public string Ocean_proximity { get; set; }
    }

    #endregion model input class
}