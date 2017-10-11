class CreatePosts < ActiveRecord::Migration
  def change
    create_table :posts do |t|
      t.string :title
      t.string :author
      t.text :intro
      t.text :extended
      t.date :published_at

      t.timestamps
    end
  end
end
