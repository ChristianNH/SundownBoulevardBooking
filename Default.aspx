<%@ Page Title="Sundown Boulevard booking" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">


	<div id="BookingApp" class="content">
		
		<div id="places">
			<div class="row">
				<div class="col-12 mb-4">
					<h1>Book this Restaurant</h1>
				</div>
			</div>
			<div class="row">
				<div class="col-4">
					<select v-model="PartySize" id="choosePartySize">
						<option value="2">For 2</option>
						<option value="3">For 3</option>
						<option value="4">For 4</option>
						<option value="5">For 5</option>
						<option value="6">For 6</option>
						<option value="7">For 7</option>
						<option value="8">For 8</option>
						<option value="9">For 9</option>
						<option value="10">For 10</option>
					</select>
				</div>
				<div class="col-4">
					<input id="dateSelector" type="text" />
				</div>
				<div class="col-4">
					<select v-model="SelectedTime" id="chooseTimeslot">
						<option value="16:00">16:00</option>
						<option value="16:30">16:30</option>
						<option value="17:00">17:00</option>
						<option value="17:30">17:30</option>
						<option value="18:00">18:00</option>
						<option value="18:30">18:30</option>
						<option selected="selected" value="19:00">19:00</option>
						<option value="19:30">19:30</option>
						<option value="20:00">20:00</option>
					</select>
				</div>
			</div>
		</div>
		<div class="row">
			<div class="col-12">
				<span v-if="TimeslotError" class="error-msg mt-2">
					<br />
					Sorry! There are no tables matching your search.
				</span>
				<span v-if="TimeslotAvailable" class="success-msg mt-2">
					<br />
					Table available!
				</span>
			</div>
			<div class="col-12">
				
				<div v-if="!TimeslotAvailable" class="btn" v-on:click="CheckTimeslot()">
					Find a table
				</div>
			</div>
		</div>
		<template v-if="Step != 'confirmbooking'">
		<div v-if="TimeslotAvailable" class="detailsForm">
			<div class="row">
				<div class="col-6">
					<label for="txtName">Name:</label><br />
					<input id="txtName" v-model="Name" type="text" />
				</div>
				<div class="col-6">
					<label for="txtEmail">Email:</label><br />
					<input id="txtEmail" v-model="Email" type="email" />
				</div>
			</div>
			<div class="row">
				<div class="col-6">
					<label for="txtPhone">Phone:</label><br />
					<input v-model="Phone" type="tel" />
				</div>
			</div>

			<template v-if="SelectedDrink == null">
				<div v-if="Drinks" id="chooseDrink" class="row mt-4 pt-4">

					<div class="col-12">
						<h2>Choose a drink</h2>
					</div>
					<div class="d-flex justify-content-between align-items-center col-12 mb-2">

						<div v-on:click="GetDrinksPagingDecrease" class="btn btn-group">
							Previous
						</div>
						<div v-on:click="GetDrinksPagingIncrease" class="btn btn-group">
							Next
						</div>
					</div>

					<div v-for="Drink in Drinks" class="col-md-4">
						<div class="card mb-4 box-shadow">
							<img class="card-img-top" v-bind:src="Drink.image_url" alt="Thumbnail [100%x225]" />
							<div class="card-body">
								<p class="card-text">
									<b>{{Drink.name}}</b><br />
									{{Drink.description}}

								</p>
								<div class="d-flex justify-content-between align-items-center">
									<div class="btn-group">
										<button @click="SelectedDrink = Drink" type="button" class="btn btn-sm btn-outline-secondary">Select</button>
									</div>

								</div>
							</div>
						</div>
					</div>
				</div>
			</template>
			<template v-if="SelectedDrink != null">
				<div v-if="Dishes" id="chooseDish" class="row mt-4 pt-4">

					<div class="col-12 mb-2">
						<h2>Choose a dish</h2>
					</div>

					<div v-for="Dish in Dishes" class="col-md-4 ">
						<div class="card mb-4 box-shadow">
							<img class="card-img-top" v-bind:src="Dish.strMealThumb" alt="Thumbnail [100%x225]" />
							<div class="card-body">
								<p class="card-text">
									<b>{{Dish.strMeal}}</b>
								</p>
								<div class="d-flex justify-content-between align-items-center">
									<div class="btn-group">
										<button @click="SelectedDish = Dish" type="button" class="btn btn-sm btn-outline-secondary">Select</button>
									</div>

								</div>
							</div>
						</div>
					</div>
				</div>
			</template>
			

		</div>

		</template>
		<div v-if="Step" class="chosenInformation row">
			<div class="row">
				<div class="col-6">
					<b>{{ Name }} </b>
				</div>
				<div class="col-6">
					<b>	{{ Email }}</b>
				</div>
			</div>
			
			{{ SelectedDate }} {{ SelectedTime }}<br />
			For {{ PartySize }}
			<div v-if="SelectedDrink" class="row">
					<div class="col-4">
						<img class="card-img-top" v-bind:src="SelectedDrink.image_url" alt="" />
					</div>
					<div style="align-self: center;" class="col-8">
						<p class="card-text">
							<b>{{SelectedDrink.name}}</b><br />
							{{SelectedDrink.description}}
						</p>
					</div>
			</div>
			<div v-if="SelectedDish" class="row">
					<div class="col-4">
						<img style="width:100%;" class="card-img-top" v-bind:src="SelectedDish.strMealThumb" alt="" />
					</div>
					<div style="align-self: center;" class="col-8">
						<p class="card-text">
							<b>{{SelectedDish.strMeal}}</b>
						</p>
					</div>
			</div>
		</div>
		<div v-if="Step == 'confirmbooking'">
			<div class="row mb-4">
				<div class="col-12">
					<div class="btn" v-on:click="BookTable()">
						Confirm booking
					</div>
				</div>
			</div>
		</div>
	</div>

</asp:Content>

