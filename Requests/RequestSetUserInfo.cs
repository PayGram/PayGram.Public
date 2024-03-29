﻿using System.ComponentModel.DataAnnotations;

namespace PayGram.Public.Requests
{
	public class RequestSetUserInfo : PaygramRequest
	{
		[StringLength(Constraints.CALLBACKURL_MAX)]
		public string CallbackUrl { get; set; }
	}
}
