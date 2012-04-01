hi,

mvc project awesome requires jquery, jquery ui and jquery.form

(jquery.form was copied by MvcProjectAwesome into your Scripts folder)

I didn't added them (jqueries) as dependencies because some people (me) use cdn refs like below

<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/ui-lightness/jquery-ui.css" rel="stylesheet" type="text/css" />
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js" type="text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/jquery-ui.min.js" type="text/javascript"></script>

so you also need to add jquery, jqueryui.js/css and jquery.form in your Site.master/Layout.cshtml

if you're using razor, look here for additional setup: http://awesome.codeplex.com/wikipage?title=Installation


project website: 

http://awesome.codeplex.com
