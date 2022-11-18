using Database;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Extension
{
    public static class Extensions
    {
        public static ErrorModel ErrorObject(this Exception exception)
        {
            try
            {
                var ex = exception;
                StringBuilder builder = new StringBuilder();
                List<ValueTuple<string, string>> exceptions = new List<ValueTuple<string, string>>();

                var message = ex.Message;
                var type = ex.GetType().Name;
                var caption = string.Concat(type.Select(x => char.IsUpper(x) ? $" {x}" : x.ToString())).Trim();
                builder.Append($"{caption}: {message}");

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message = ex.Message;
                    type = ex.GetType().Name;
                    caption = string.Concat(type.Select(x => char.IsUpper(x) ? $" {x}" : x.ToString())).Trim();
                    exceptions.Add((caption, message));
                }

                foreach (var item in exceptions)
                {
                    var line = $" -- {item.Item1}: {item.Item2}";
                    builder.AppendLine(line);
                }

                return new ErrorModel { Caption = "Low Level Error Occured", Message = builder.ToString() };
            }
            catch (Exception ex)
            {
                return new ErrorModel { Caption = "Exepction Parsing Error", Message = ex.Message };
            }

            //Exception ex = exception;

            //while (ex.InnerException != null)
            //{
            //    ex = ex.InnerException;
            //}

            //var message = ex.Message;
            //var type = ex.GetType().Name;
            //var caption = string.Concat(type.Select(x => char.IsUpper(x) ? $" {x}" : x.ToString())).Trim();
            //return new ErrorModel { Caption = caption, Message = message };
        }

        public static List<TResult> GetPage<TResult>(this IEnumerable<TResult> data, PaginationBaseModel model) where TResult : class
        {
            return data.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
        }

        public static async Task<List<TResult>> GetPageAsync<TResult>(this IQueryable<TResult> data, PaginationBaseModel model) where TResult : class
        {
            return await data.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
        }

    }
}
