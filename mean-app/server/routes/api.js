const express = require('express');
const router = express.Router();

const http = require('axios');
const API = 'https://jsonplaceholder.typicode.com'

router.get('/', (req, res) => {
  res.send('api works');
});

router.get('/posts', (req, res) => {
  http.get(`${API}/posts`)
    .then(posts => {
      res.status(200).json(posts.data);
    })
    .catch(error => {
      res.status(500).json(error);
    });
});

module.exports = router;