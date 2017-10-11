namespace 'db' do
  task populate: :environment do
    10.times {
      Post.create title: Faker::Lorem.sentence(4),
                  author: Faker::Name.name,
                  intro: Faker::Lorem.sentence(10),
                  extended: Faker::Lorem.paragraph(10),
                  published_at: Time.now
    }
  end 
end