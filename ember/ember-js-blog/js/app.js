App = Ember.Application.create();

App.Store = DS.Store.extend({
  revision: 12,
  adapter:  'DS.FixtureAdapter'
});

App.Router.map(function() {
  this.resource('posts', function() {
    this.resource('post', { path: ':post_id' })
  });
  this.resource('about');
});

App.IndexRoute = Ember.Route.extend({
  redirect: function() {
    this.transitionTo('posts');
  }
});

App.PostsRoute = Ember.Route.extend({
  model: function() {
      return App.Post.find();
  }
});

App.PostController = Ember.ObjectController.extend({
  isEditing: false,

  doneEditing: function() {
    this.set('isEditing', false);
  },

  edit: function() {
    this.set('isEditing', true);
  }
});

App.Post = DS.Model.extend({
  title:        DS.attr('string'),
  author:       DS.attr('string'),
  intro:        DS.attr('string'),
  extended:    DS.attr('string'),
  publishedAt:  DS.attr('date')
});

App.Post.FIXTURES = [{
  id: 1,
  title: "Post 1",
  author: "omar",
  publishedAt: new Date('12-27-2013'),
  intro:  "This is test data for the ember.js tutorial",
  extended: "<h1>More</h1> text to use as fixture data, lalala, bla bla bla"
}, {
  id: 2,
  title: "Post 2",
  author: "javier",
  publishedAt: new Date('12-27-2013'),
  intro:  "This is test data for the ember.js tutorial",
  extended: "More text to use as fixture data, lalala, bla bla bla"
}, {
  id: 3,
  title: "Post 3",
  author: "john",
  publishedAt: new Date('12-27-2013'),
  extended: "More text to use as fixture data, lalala, bla bla bla",
  intro:  "This is test data for the ember.js tutorial"
}];

Ember.Handlebars.registerBoundHelper('date', function(date) {
  return moment(date).fromNow();
});

var showdown = new Showdown.converter();
Ember.Handlebars.registerBoundHelper('markdown', function(input) {
  return new Ember.Handlebars.SafeString(showdown.makeHtml(input));
});