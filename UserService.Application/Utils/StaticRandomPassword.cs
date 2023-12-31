﻿using UserService.Domain.User;

namespace UserService.Application.Utils;

public static class StaticRandomPassword
{
    private const string RandomPassword =
        "xoBj4Q5h0A+vCZ/oMIQvWCNZDKydhSY6RCXhhGjfbs8=:/GZMs9GB/LimGxUEseU058Z0IY2yBRN2nkBER891Gcf/sUnksavvp2jobIOjtPdQJ5mrppr0xNUnO5bUzsuv8Od2bvrDke/fckA7kSpTHH6+E2/Za4TuQgLkJNN0aaMRmi007IK+mf5ECuUKkQjcq1cQJX6SI1L2kra23gy3wnTYjK4vbjjqp4+V3/hSuiX8CdTfJdAohNKZnlqWn9mO25kn1T/XWHeyJ/vnlP3r5K2vLyL5DBLe1hKsZYf1X+Y1qlasPkZLsOJh6SKRafi1M1s3y8xfRTyLiq1UEk+itR6Spv3feShcbIfjiRHBtHf9DIOGx2E6/9XvxbCebdnRouDnI0UG/eOMS1gEA0YmfDAP6z+R9uEPWugxdraXlzylRDYltIxSC0yEw6o1ypPcIEub+hjCXNrCWi/J9HWMqp90IEHX7L6HwaczcGOhFo86wlbntCrJWoYDfBxhIAVbJy5PSaQ6FS/LxsCPu3Ux/ayGFNu2lpdn1pqdDNDhf74wcjzHPfHS2Y05+TTvyu8eBW3s8oWAcWIsM4AJYvAV3+I2xSn01LrJ9cpiP1QzXQ58EgGLpbfLd8Vu3G3jLh6imobOj2OGpbTvGsegq2qlqTv5WBbu+l4NKgo+YnlcUTVRg29lrM11WQK6vBtONZ5v/Ove5oYyEiE5WgF4FvGeGH6w26vsjhL4cFapqjeprTR66liYEdGq4Ir2qwlQ0/5pUwCK+j3K/ennqTbIKJRGKO96sTWI713FlWtvY2Y+np087X2VIR3keY1fG9mVZjSKTfLAIHo78cdFUYyN2nfCtI3wXZk0SvJFRYUjyfzJp8ks9eFkPdwWOlr4P27dCClLXghzR6TNctWMxSJHHCwjQHyMYMI8sxDoeMvOB1IjwhamSuFgIpDFC+VMzadNdhuFxiFIwJ48FzhDB5BC8rfI7zdNMmrgS27s1rsmseXS/4JcXfP2GTE+lPnUkwbVsT9I6lgzePKkCSu/+bc9WYBof06f5xZdjc4Be13asAWoTpz/DVQ5covowOM2qxPJn9BpjDMwrdc3UBhHCiYlZloBkX964xggGaO5BKZZ745QjgKyPw/pD5CTHjl9bBQvshidL/RtUUBzcV0QaIbeXVN231LlhcPI5qYEkgQdv4r+QbrBDKEEvsP3xAYkNN7kNzm1m2U1nZrIudFk1JVyPiWamSIKGesI4f3Et4kgYfik2IuQuAfrdTCvbOvv4+gsTJ7wz6YKWShPW5YUFxscpwTtwblEBXZokPjV96mGd/c3FMPZ/n/w38DMLFOriQ+pNK0cYF4NIFopxxMeXoftoV9+1NHioTJDBswXFt3AX/4rKak5+mkfo9bypaWIUX990xeopA==";

    public static Password Value { get; } = Password.Parse(RandomPassword);
}