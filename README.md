# Line-Drawing-Algorithm
A little game for showcasing the Line Drawing Algorithm

private IEnumerable<Point> DrawingLine(int x, int y, int x2, int y2)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                yield return new Point(x, y);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
        
# Contacts

My Facebook Profile:
https://www.facebook.com/mitko.nikov.3

Facebook Page for C# Programming:
https://www.facebook.com/nikovcsharp


My e-mail:
mitkonikov01@gmail.com

My Website:
http://mitkonikov01.wix.com/mitkonikov

Youtube channel:
https://www.youtube.com/user/MitkoMr

Twitter:
@mitkonikov



Tumblr:

3D Modelling with Mitko:
http://mitkonikov.tumblr.com/

Programming:
http://nprogramming.tumblr.com/



GitHub:
https://github.com/mitkonikov
