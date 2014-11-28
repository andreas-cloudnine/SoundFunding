<!DOCTYPE HTML>
<head>
<title>index | Tele2</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport" content="width=device-width, initial-scale=1">
<link type="text/css" rel="stylesheet" href="style.css" media="all"></head>



<body>

<style>
@media (max-width: 680px) {
	#circle {
		top:150px;
	}
}
@media (min-width: 601px) {
	#circle {
		top:175px;
	}
}
@media (min-width: 1200px) {
	#circle {
		top:200px;
	}
}
</style>


<div class="logo">
	<img src="img/logotype.png">
	<div style="float:right; padding-right:20px;">
		<img src="img/logotype.filtr.png">
	</div>
</div>

<div class="container" style="margin-top:50px; background:#0072B1;">
	<div class="top" style="display:none;">

	</div>
</div>

<?php //include "menu.php" ?>

<div class="clear"></div>

<img src="img/andreas.jpg" id="circle">

<div class="container" style="margin-top:250px;">
	<div class="content clean">
		<h1>Hi, Andreas Bjurenborg!</h1>
	
	</div>
</div>

<div class="container">
	<div class="content clean">

		<div class="form">
			<fieldset>
				<input type="text" placeholder="Name YOUR funding!">
				<textarea placeholder="Describe your funding with two or three sentences"></textarea>
				<select class="selectpicker show-menu-arrow">
					<option value="goal">Set your goal</option>
					<option value="">100 kr</option>
					<option value="">200 kr</option>
				</select>
				<h2>Upload your photo (a BIG one)</h2>
				<a href="">
					<img src="img/icon.camera.png" alt="">
				</a>
				<h2>Choose your organization</h2>
				<div class="logos">
					<label>
						<img src="img/logotype.bris.png" alt="BRIS" />
						<input type="radio" name="radio">
					</label>	
					<label>
						<img src="img/logotype.amnesty.png" alt="Amnesty International" />
						<input type="radio" name="radio">
					</label>
					<label>
						<img src="img/logotype.actionaid.png" alt="Actionaid" />
						<input type="radio" name="radio">
					</label>
					<label>
						<img src="img/logotype.wwf.png" alt="WWF" />
						<input type="radio" name="radio">
					</label>
				</div>
				<a href="" class="button">Create your soundfund</a>
			</fieldset>

		</div>	
		
		<div class="clear"></div>
	</div>
</div>


<div class="footer">
	<div class="menu">
		<ul>
			<li><a href='_login.php'>Footer Link</a></li>
		</ul>
	</div>
</div>

		<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>
		<!--<script type="text/javascript" src="http://netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
		<script type="text/javascript" src="scripts/bootstrap-select.js"></script>
		<script type="text/javascript">
			$('.selectpicker').selectpicker();
		</script>-->

</body>
</html>